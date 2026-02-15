using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Enums;
using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for accessing CSGOEmpire trade, withdrawal, and marketplace endpoints.
/// </summary>
internal sealed class TradeService : ITradeService
{
    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<TradeService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TradeService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public TradeService(CsGoEmpireHttpClient httpClient, ILogger<TradeService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ActiveTradesResponse> GetActiveTradesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving active trades");

        var response = await _httpClient
            .GetAsync<ActiveTradesResponse>("trading/user/trades", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Active trades retrieved — {Count} trade(s)", response.Data?.Count ?? 0);

        return response;
    }

    /// <inheritdoc />
    public async Task<TradeResponse> GetTradeAsync(
        int depositId,
        TradeType type,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);

        var typeString = type switch
        {
            TradeType.Bid => "bid",
            TradeType.Deposit => "deposit",
            TradeType.Withdrawal => "withdrawal",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid trade type.")
        };

        _logger.LogInformation("Retrieving trade {DepositId} of type {Type}", depositId, typeString);

        var response = await _httpClient
            .GetAsync<TradeResponse>($"trading/user/trade/{depositId}/{typeString}", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Trade retrieved — ID: {Id}, Status: {Status}",
            response.Data?.Id, response.Data?.StatusMessage);

        return response;
    }

    /// <inheritdoc />
    public async Task<ListedItemsResponse> GetListedItemsAsync(
        GetListedItemsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Page < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request), request.Page, "Page number must be at least 1.");
        }

        if (request.PerPage < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request), request.PerPage, "PerPage must be at least 1.");
        }

        var queryString = request.ToQueryString();
        var uri = $"trading/items?{queryString}";

        _logger.LogInformation(
            "Retrieving listed items (page: {Page}, perPage: {PerPage})",
            request.Page, request.PerPage);

        var response = await _httpClient
            .GetAsync<ListedItemsResponse>(uri, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Listed items retrieved — page {CurrentPage}/{LastPage}, {Count} item(s)",
            response.CurrentPage, response.LastPage, response.Data?.Count ?? 0);

        return response;
    }

    /// <inheritdoc />
    public async Task<TradeResponse> CreateWithdrawalAsync(
        int depositId,
        CreateWithdrawalRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);

        _logger.LogInformation(
            "Creating withdrawal for deposit {DepositId} (coinValue: {CoinValue})",
            depositId, request?.CoinValue?.ToString() ?? "default");

        var response = await _httpClient
            .PostAsync<TradeResponse>($"trading/deposit/{depositId}/withdraw", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Withdrawal created — trade ID: {TradeId}, status: {Status}",
            response.Data?.Id, response.Data?.StatusMessage);

        return response;
    }

    /// <inheritdoc />
    public async Task MarkAsSentAsync(
        int depositId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);

        _logger.LogInformation("Marking deposit {DepositId} as sent", depositId);

        await _httpClient
            .PostAsync($"trading/deposit/{depositId}/sent", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Deposit {DepositId} marked as sent", depositId);
    }

    /// <inheritdoc />
    public async Task MarkAsReceivedAsync(
        int tradeofferId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tradeofferId);

        _logger.LogInformation("Marking trade offer {TradeofferId} as received", tradeofferId);

        await _httpClient
            .PostAsync($"trading/deposit/{tradeofferId}/received", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Trade offer {TradeofferId} marked as received", tradeofferId);
    }

    /// <inheritdoc />
    public async Task DisputeTradeAsync(
        int tradeofferId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tradeofferId);

        _logger.LogInformation("Disputing trade offer {TradeofferId}", tradeofferId);

        await _httpClient
            .PostAsync($"trading/deposit/{tradeofferId}/dispute", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Trade offer {TradeofferId} disputed successfully", tradeofferId);
    }
}
