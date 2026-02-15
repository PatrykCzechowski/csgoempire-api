using System.Text.Json;
using CsGoEmpire.Api.Configuration;
using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Responses;
using CsGoEmpire.Api.Models.WebSocket;
using CsGoEmpire.Api.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocketIOClient;
using SocketIOClient.Transport;

namespace CsGoEmpire.Api.WebSocket;

/// <summary>
/// WebSocket client for receiving real-time events from the CSGOEmpire trade server.
/// Implements Socket.IO v4 protocol with automatic authentication and reconnection.
/// </summary>
internal sealed class CsGoEmpireWebSocketClient : ICsGoEmpireWebSocketClient
{
    /// <summary>
    /// Maximum reconnection delay cap in milliseconds (2 minutes).
    /// </summary>
    private const int MaxReconnectDelayMs = 120_000;

    /// <summary>
    /// Base reconnection delay in milliseconds (1 second).
    /// </summary>
    private const int BaseReconnectDelayMs = 1_000;

    /// <summary>
    /// Default maximum price filter to receive all item events.
    /// </summary>
    private const int DefaultPriceMaxFilter = 9999999;

    private readonly IMetadataService _metadataService;
    private readonly CsGoEmpireOptions _options;
    private readonly ILogger<CsGoEmpireWebSocketClient> _logger;

    private SocketIOClient.SocketIO? _socket;
    private MetadataResponse? _metadata;
    private bool _disposed;
    private bool _isAuthenticated;
    private int _reconnectAttempts;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private CancellationTokenSource? _reconnectCts;

    /// <summary>
    /// Initializes a new instance of the <see cref="CsGoEmpireWebSocketClient"/> class.
    /// </summary>
    /// <param name="metadataService">The metadata service for fetching WebSocket authentication credentials.</param>
    /// <param name="options">The CSGOEmpire API options.</param>
    /// <param name="logger">The logger instance.</param>
    public CsGoEmpireWebSocketClient(
        IMetadataService metadataService,
        IOptions<CsGoEmpireOptions> options,
        ILogger<CsGoEmpireWebSocketClient> logger)
    {
        _metadataService = metadataService;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool IsConnected => _socket?.Connected == true && _isAuthenticated;

    /// <inheritdoc />
    public event EventHandler<List<MarketItem>>? OnNewItem;

    /// <inheritdoc />
    public event EventHandler<List<MarketItem>>? OnUpdatedItem;

    /// <inheritdoc />
    public event EventHandler<List<AuctionUpdateEvent>>? OnAuctionUpdate;

    /// <inheritdoc />
    public event EventHandler<List<int>>? OnDeletedItem;

    /// <inheritdoc />
    public event EventHandler<TradeStatusEvent>? OnTradeStatus;

    /// <inheritdoc />
    public event EventHandler<DepositFailedEvent>? OnDepositFailed;

    /// <inheritdoc />
    public event EventHandler<long>? OnTimeSync;

    /// <inheritdoc />
    public event EventHandler? OnConnected;

    /// <inheritdoc />
    public event EventHandler? OnDisconnected;

    /// <inheritdoc />
    public event EventHandler<Exception>? OnError;

    /// <inheritdoc />
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _connectionLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_socket?.Connected == true)
            {
                throw new InvalidOperationException(
                    "WebSocket client is already connected. Call DisconnectAsync() first.");
            }

            _reconnectCts?.Cancel();
            _reconnectCts?.Dispose();
            _reconnectCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _reconnectAttempts = 0;

            await ConnectInternalAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _connectionLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            _reconnectCts?.Cancel();
            _reconnectCts?.Dispose();
            _reconnectCts = null;

            if (_socket is not null)
            {
                _logger.LogInformation("Disconnecting from CSGOEmpire WebSocket");

                await _socket.DisconnectAsync().ConfigureAwait(false);
                _socket.Dispose();
                _socket = null;
            }

            _isAuthenticated = false;
            _logger.LogDebug("WebSocket disconnected");
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    /// <summary>
    /// Internal method that performs the full connection sequence: metadata fetch, Socket.IO connect, event binding.
    /// </summary>
    private async Task ConnectInternalAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching WebSocket metadata for authentication");
        _metadata = await _metadataService.GetSocketMetadataAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogDebug(
            "Metadata retrieved — UID: {Uid}, SocketToken length: {TokenLength}",
            _metadata.User.Id, _metadata.SocketToken.Length);

        // Dispose existing socket if any
        if (_socket is not null)
        {
            _socket.Dispose();
            _socket = null;
        }

        // Parse WebSocket URL to extract base URI (SocketIOClient uses URI without query string for some params)
        var wsUri = BuildWebSocketUri();

        _socket = new SocketIOClient.SocketIO(wsUri, new SocketIOOptions
        {
            Transport = TransportProtocol.WebSocket,
            Path = "/s/",
            Reconnection = false, // We handle reconnection ourselves for metadata refresh
            Query = new Dictionary<string, string>
            {
                ["uid"] = _metadata.User.Id.ToString(),
                ["token"] = _metadata.SocketToken
            },
            ExtraHeaders = new Dictionary<string, string>
            {
                ["User-agent"] = $"{_metadata.User.Id} API Bot"
            }
        });

        RegisterEventHandlers();

        _logger.LogInformation("Connecting to CSGOEmpire WebSocket at {Uri}", wsUri);
        await _socket.ConnectAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Socket.IO transport connected, awaiting init event");
    }

    /// <summary>
    /// Constructs the WebSocket base URI from the configured URL.
    /// </summary>
    private string BuildWebSocketUri()
    {
        // SocketIOClient expects the base URI without path/query — we set path/query via options
        // The configured URL is like: wss://trade.csgoempire.com/s/?EIO=3&transport=websocket
        // We need: https://trade.csgoempire.com
        var configuredUrl = _options.WebSocketUrl;

        // Try to convert wss -> https for SocketIOClient
        if (configuredUrl.StartsWith("wss://", StringComparison.OrdinalIgnoreCase))
        {
            configuredUrl = "https://" + configuredUrl[6..];
        }
        else if (configuredUrl.StartsWith("ws://", StringComparison.OrdinalIgnoreCase))
        {
            configuredUrl = "http://" + configuredUrl[5..];
        }

        // Extract just the base authority
        var uri = new Uri(configuredUrl);
        return $"{uri.Scheme}://{uri.Authority}";
    }

    /// <summary>
    /// Registers all Socket.IO event handlers on the current socket instance.
    /// </summary>
    private void RegisterEventHandlers()
    {
        if (_socket is null) return;

        _socket.OnConnected += (_, _) =>
        {
            _logger.LogDebug("Socket.IO connected event received");
        };

        _socket.OnDisconnected += (_, reason) =>
        {
            _logger.LogWarning("WebSocket disconnected: {Reason}", reason);
            _isAuthenticated = false;
            OnDisconnected?.Invoke(this, EventArgs.Empty);

            // Trigger auto-reconnect
            _ = TryReconnectAsync();
        };

        _socket.OnError += (_, error) =>
        {
            _logger.LogError("WebSocket error: {Error}", error);
            OnError?.Invoke(this, new InvalidOperationException(error));
        };

        // init — server requests identification or confirms authentication
        _socket.On("init", async response =>
        {
            try
            {
                var json = response.GetValue<JsonElement>();
                var authenticated = json.TryGetProperty("authenticated", out var authProp) &&
                                    authProp.GetBoolean();

                if (authenticated)
                {
                    var name = json.TryGetProperty("name", out var nameProp)
                        ? nameProp.GetString() ?? "unknown"
                        : "unknown";

                    _logger.LogInformation("WebSocket authenticated as {Name}", name);
                    _isAuthenticated = true;
                    _reconnectAttempts = 0;

                    // Emit default filters to receive item events
                    await EmitFiltersAsync().ConfigureAwait(false);
                    OnConnected?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _logger.LogDebug("WebSocket init received — sending identify payload");
                    await EmitIdentifyAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling init event");
                OnError?.Invoke(this, ex);
            }
        });

        // timesync — server timestamp synchronization
        _socket.On("timesync", response =>
        {
            try
            {
                var timestamp = response.GetValue<long>();
                _logger.LogDebug("Timesync received: {Timestamp}", timestamp);
                OnTimeSync?.Invoke(this, timestamp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling timesync event");
                OnError?.Invoke(this, ex);
            }
        });

        // new_item — new marketplace items available
        _socket.On("new_item", response =>
        {
            try
            {
                var items = DeserializeItemList<MarketItem>(response);
                _logger.LogDebug("new_item received — {Count} item(s)", items.Count);
                OnNewItem?.Invoke(this, items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling new_item event");
                OnError?.Invoke(this, ex);
            }
        });

        // updated_item — existing items updated
        _socket.On("updated_item", response =>
        {
            try
            {
                var items = DeserializeItemList<MarketItem>(response);
                _logger.LogDebug("updated_item received — {Count} item(s)", items.Count);
                OnUpdatedItem?.Invoke(this, items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling updated_item event");
                OnError?.Invoke(this, ex);
            }
        });

        // auction_update — bid updates on auction items
        _socket.On("auction_update", response =>
        {
            try
            {
                var updates = DeserializeItemList<AuctionUpdateEvent>(response);
                _logger.LogDebug("auction_update received — {Count} update(s)", updates.Count);
                OnAuctionUpdate?.Invoke(this, updates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling auction_update event");
                OnError?.Invoke(this, ex);
            }
        });

        // deleted_item — items removed from marketplace
        _socket.On("deleted_item", response =>
        {
            try
            {
                var ids = DeserializeItemList<int>(response);
                _logger.LogDebug("deleted_item received — {Count} ID(s)", ids.Count);
                OnDeletedItem?.Invoke(this, ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling deleted_item event");
                OnError?.Invoke(this, ex);
            }
        });

        // trade_status — trade status updates
        _socket.On("trade_status", response =>
        {
            try
            {
                var tradeEvent = DeserializeSingleOrFirst<TradeStatusEvent>(response);
                if (tradeEvent is not null)
                {
                    _logger.LogDebug(
                        "trade_status received — type: {Type}, ID: {Id}, status: {Status}",
                        tradeEvent.Type, tradeEvent.Data?.Id, tradeEvent.Data?.StatusMessage);
                    OnTradeStatus?.Invoke(this, tradeEvent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling trade_status event");
                OnError?.Invoke(this, ex);
            }
        });

        // deposit_failed — deposit processing failures
        _socket.On("deposit_failed", response =>
        {
            try
            {
                var failedEvent = DeserializeSingleOrFirst<DepositFailedEvent>(response);
                if (failedEvent is not null)
                {
                    _logger.LogWarning(
                        "deposit_failed received — item: {ItemId}, error: {ErrorKey}, message: {Message}",
                        failedEvent.Response?.Data?.ItemId,
                        failedEvent.Response?.Data?.ErrorKey,
                        failedEvent.Response?.Data?.Message);
                    OnDepositFailed?.Invoke(this, failedEvent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling deposit_failed event");
                OnError?.Invoke(this, ex);
            }
        });
    }

    /// <summary>
    /// Emits the identify payload to authenticate the WebSocket connection.
    /// </summary>
    private async Task EmitIdentifyAsync()
    {
        if (_socket is null || _metadata is null) return;

        var payload = new IdentifyPayload
        {
            Uid = _metadata.User.Id,
            Model = _metadata.User,
            AuthorizationToken = _metadata.SocketToken,
            Signature = _metadata.SocketSignature
        };

        _logger.LogDebug("Emitting identify for UID {Uid}", payload.Uid);
        await _socket.EmitAsync("identify", payload).ConfigureAwait(false);
    }

    /// <summary>
    /// Emits the default filters payload to start receiving item events.
    /// </summary>
    private async Task EmitFiltersAsync()
    {
        if (_socket is null) return;

        var filters = new FiltersPayload { PriceMax = DefaultPriceMaxFilter };
        _logger.LogDebug("Emitting filters (price_max: {PriceMax})", filters.PriceMax);
        await _socket.EmitAsync("filters", filters).ConfigureAwait(false);
    }

    /// <summary>
    /// Attempts to reconnect with exponential backoff. Refreshes metadata on each attempt
    /// since the socket token is short-lived (~30 seconds).
    /// </summary>
    private async Task TryReconnectAsync()
    {
        var cts = _reconnectCts;
        if (cts is null || cts.IsCancellationRequested) return;

        try
        {
            _reconnectAttempts++;
            var delay = CalculateReconnectDelay(_reconnectAttempts);

            _logger.LogInformation(
                "Attempting reconnection #{Attempt} in {Delay}ms",
                _reconnectAttempts, delay);

            await Task.Delay(delay, cts.Token).ConfigureAwait(false);

            if (cts.IsCancellationRequested) return;

            await _connectionLock.WaitAsync(cts.Token).ConfigureAwait(false);
            try
            {
                await ConnectInternalAsync(cts.Token).ConfigureAwait(false);
            }
            finally
            {
                _connectionLock.Release();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("Reconnection cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reconnection attempt #{Attempt} failed", _reconnectAttempts);
            OnError?.Invoke(this, ex);

            // Schedule next reconnection attempt
            _ = TryReconnectAsync();
        }
    }

    /// <summary>
    /// Calculates the reconnection delay using exponential backoff with jitter.
    /// </summary>
    /// <param name="attempt">The current reconnection attempt number (1-based).</param>
    /// <returns>The delay in milliseconds before the next reconnection attempt.</returns>
    private static int CalculateReconnectDelay(int attempt)
    {
        // Exponential backoff: 1s, 2s, 4s, 8s, 16s, 32s, 64s, 120s (capped)
        var exponentialDelay = BaseReconnectDelayMs * (1 << Math.Min(attempt - 1, 10));
        var cappedDelay = Math.Min(exponentialDelay, MaxReconnectDelayMs);

        // Add jitter (±25%) to prevent thundering herd
        var jitter = Random.Shared.Next(-cappedDelay / 4, cappedDelay / 4);
        return Math.Max(BaseReconnectDelayMs, cappedDelay + jitter);
    }

    /// <summary>
    /// Deserializes a Socket.IO event payload that can be either a single item or an array.
    /// Always returns a list for uniform handling.
    /// </summary>
    private static List<T> DeserializeItemList<T>(SocketIOResponse response)
    {
        var json = response.GetValue<JsonElement>();

        if (json.ValueKind == JsonValueKind.Array)
        {
            return json.Deserialize<List<T>>(CsGoEmpireHttpClient.JsonOptions) ?? [];
        }

        // Single item — wrap in list
        var item = json.Deserialize<T>(CsGoEmpireHttpClient.JsonOptions);
        return item is not null ? [item] : [];
    }

    /// <summary>
    /// Deserializes a Socket.IO event payload that can be either a single item or an array,
    /// returning the first (or only) element.
    /// </summary>
    private static T? DeserializeSingleOrFirst<T>(SocketIOResponse response) where T : class
    {
        var json = response.GetValue<JsonElement>();

        if (json.ValueKind == JsonValueKind.Array)
        {
            if (json.GetArrayLength() == 0) return null;
            return json[0].Deserialize<T>(CsGoEmpireHttpClient.JsonOptions);
        }

        return json.Deserialize<T>(CsGoEmpireHttpClient.JsonOptions);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        _reconnectCts?.Cancel();
        _reconnectCts?.Dispose();
        _reconnectCts = null;

        if (_socket is not null)
        {
            try
            {
                if (_socket.Connected)
                {
                    await _socket.DisconnectAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error during WebSocket disposal disconnect");
            }
            finally
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        _connectionLock.Dispose();
        _logger.LogDebug("CsGoEmpireWebSocketClient disposed");
    }
}
