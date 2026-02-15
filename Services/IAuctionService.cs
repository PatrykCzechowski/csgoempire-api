using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire auction endpoints.
/// </summary>
public interface IAuctionService
{
    /// <summary>
    /// Retrieves all active auctions for the authenticated user.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/user/auctions</c>
    /// <para>
    /// Returns all items currently being auctioned by the authenticated user.
    /// Use the WebSocket <c>auction_update</c> event for real-time bid updates.
    /// </para>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>An <see cref="AuctionsResponse"/> containing the list of active auction items.</returns>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<AuctionsResponse> GetActiveAuctionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Places a bid on an active auction item.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{deposit_id}/bid</c>
    /// <para>
    /// The bid value must be greater than 0 and is specified in coincents (1 coin = 100 coincents).
    /// The bid must exceed the current highest bid for the auction.
    /// </para>
    /// </remarks>
    /// <param name="depositId">The deposit ID of the auction item to bid on.</param>
    /// <param name="request">The request containing the bid value.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <see cref="PlaceBidRequest.BidValue"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task PlaceBidAsync(int depositId, PlaceBidRequest request, CancellationToken cancellationToken = default);
}
