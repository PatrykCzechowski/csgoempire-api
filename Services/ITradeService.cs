using CsGoEmpire.Api.Models.Enums;
using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire trade, withdrawal, and marketplace endpoints.
/// </summary>
public interface ITradeService
{
    /// <summary>
    /// Retrieves all active trades for the authenticated user.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/user/trades</c>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>An <see cref="ActiveTradesResponse"/> containing the list of active trades.</returns>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<ActiveTradesResponse> GetActiveTradesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the status of a specific trade by its deposit/trade/bid ID and type.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/user/trade/{depositId}/{type}</c>
    /// <para>
    /// This should be used as a backup to the WebSocket for checking trade status.
    /// The <paramref name="depositId"/> can be a deposit ID, a trade ID, or a bid ID.
    /// </para>
    /// </remarks>
    /// <param name="depositId">The deposit, trade, or bid ID.</param>
    /// <param name="type">The trade type (bid, deposit, or withdrawal).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="TradeResponse"/> containing the trade data.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<TradeResponse> GetTradeAsync(int depositId, TradeType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves listed marketplace items with optional filtering and pagination.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/items</c>
    /// <para>
    /// Supports extensive filtering by price, wear, stickers, delivery time, and more.
    /// </para>
    /// </remarks>
    /// <param name="request">The request containing filter and pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="ListedItemsResponse"/> containing the paginated marketplace items.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <see cref="GetListedItemsRequest.Page"/> or <see cref="GetListedItemsRequest.PerPage"/> is less than 1.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<ListedItemsResponse> GetListedItemsAsync(GetListedItemsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a withdrawal (purchases an item) from the marketplace.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{deposit_id}/withdraw</c>
    /// <para>
    /// Withdraws an item directly if the auction has expired without being won.
    /// An optional <c>coin_value</c> can be specified as the item price.
    /// </para>
    /// </remarks>
    /// <param name="depositId">The deposit ID of the item to withdraw.</param>
    /// <param name="request">Optional request body containing the price to offer. Pass <c>null</c> for direct withdrawal.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="TradeResponse"/> containing the created withdrawal trade data.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<TradeResponse> CreateWithdrawalAsync(int depositId, CreateWithdrawalRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a deposit as sent after having sent the item via the Steam trade offer.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{deposit_id}/sent</c>
    /// </remarks>
    /// <param name="depositId">The deposit ID of the trade to mark as sent.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task MarkAsSentAsync(int depositId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a trade as received after receiving the item via the Steam trade offer.
    /// Can also be used to cancel a dispute.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{tradeoffer_id}/received</c>
    /// </remarks>
    /// <param name="tradeofferId">The trade offer ID of the trade to mark as received.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="tradeofferId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task MarkAsReceivedAsync(int tradeofferId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disputes a trade when the seller marked the trade as sent but the item was not received.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{tradeoffer_id}/dispute</c>
    /// <para>
    /// Keep false disputes to a minimum to avoid account restrictions.
    /// A dispute can be canceled by calling <see cref="MarkAsReceivedAsync"/>.
    /// If the seller has privacy protection enabled, some details (asset_id, wear) will only
    /// be returned once the trade status is "sent".
    /// </para>
    /// </remarks>
    /// <param name="tradeofferId">The trade offer ID of the trade to dispute.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="tradeofferId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task DisputeTradeAsync(int tradeofferId, CancellationToken cancellationToken = default);
}
