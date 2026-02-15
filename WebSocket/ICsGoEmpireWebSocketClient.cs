using CsGoEmpire.Api.Models.Responses;
using CsGoEmpire.Api.Models.WebSocket;

namespace CsGoEmpire.Api.WebSocket;

/// <summary>
/// Provides a real-time WebSocket connection to the CSGOEmpire trade server using Socket.IO.
/// Handles authentication, reconnection, and event dispatching for marketplace and trade events.
/// </summary>
public interface ICsGoEmpireWebSocketClient : IAsyncDisposable
{
    /// <summary>
    /// Gets a value indicating whether the WebSocket client is currently connected and authenticated.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Connects to the CSGOEmpire WebSocket server, authenticates, and begins receiving events.
    /// </summary>
    /// <remarks>
    /// The connection sequence:
    /// <list type="number">
    ///   <item>Fetches metadata (socket token + signature) from the REST API.</item>
    ///   <item>Opens a Socket.IO connection to the trade WebSocket endpoint.</item>
    ///   <item>On <c>init</c>, emits <c>identify</c> with the user's credentials.</item>
    ///   <item>Emits <c>filters</c> with <c>price_max: 9999999</c> to receive all item events.</item>
    /// </list>
    /// Auto-reconnect with exponential backoff is handled automatically.
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the connection attempt.</param>
    /// <exception cref="InvalidOperationException">Thrown when the client is already connected.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when metadata retrieval fails.</exception>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gracefully disconnects from the CSGOEmpire WebSocket server.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the disconnection.</param>
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Raised when new marketplace items become available.
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>new_item</c>. Payload is an array of <see cref="MarketItem"/> objects.
    /// </remarks>
    event EventHandler<List<MarketItem>>? OnNewItem;

    /// <summary>
    /// Raised when existing marketplace items are updated (e.g., status or price changes).
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>updated_item</c>. Payload is an array of <see cref="MarketItem"/> objects.
    /// </remarks>
    event EventHandler<List<MarketItem>>? OnUpdatedItem;

    /// <summary>
    /// Raised when auction bids are placed or updated.
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>auction_update</c>. Payload is an array of <see cref="AuctionUpdateEvent"/> objects.
    /// </remarks>
    event EventHandler<List<AuctionUpdateEvent>>? OnAuctionUpdate;

    /// <summary>
    /// Raised when marketplace items are removed (e.g., auction won and withdrawn).
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>deleted_item</c>. Payload is an array of item IDs.
    /// </remarks>
    event EventHandler<List<int>>? OnDeletedItem;

    /// <summary>
    /// Raised when a trade status is updated (deposit, withdrawal, or bid).
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>trade_status</c>. Payload contains the trade type and data.
    /// </remarks>
    event EventHandler<TradeStatusEvent>? OnTradeStatus;

    /// <summary>
    /// Raised when a deposit fails processing.
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>deposit_failed</c>. Payload contains the error details.
    /// </remarks>
    event EventHandler<DepositFailedEvent>? OnDepositFailed;

    /// <summary>
    /// Raised when a time synchronization event is received from the server.
    /// </summary>
    /// <remarks>
    /// WebSocket event: <c>timesync</c>. Payload is the server's Unix timestamp in milliseconds.
    /// This event is only emitted when the client requests it by emitting a <c>timesync</c> event.
    /// </remarks>
    event EventHandler<long>? OnTimeSync;

    /// <summary>
    /// Raised when the WebSocket connection is established and the user is authenticated.
    /// </summary>
    event EventHandler? OnConnected;

    /// <summary>
    /// Raised when the WebSocket connection is lost.
    /// </summary>
    event EventHandler? OnDisconnected;

    /// <summary>
    /// Raised when a WebSocket error occurs.
    /// </summary>
    event EventHandler<Exception>? OnError;
}
