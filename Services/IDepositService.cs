using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire inventory and deposit management endpoints.
/// </summary>
public interface IDepositService
{
    /// <summary>
    /// Retrieves the authenticated user's CS2 inventory from Steam. Results are cached server-side for 1 hour.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/user/inventory</c>
    /// </remarks>
    /// <param name="invalid">
    /// When set to <c>"yes"</c>, filters out invalid items. Defaults to no filtering when <c>null</c>.
    /// </param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>An <see cref="InventoryResponse"/> containing the user's inventory items.</returns>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<InventoryResponse> GetInventoryAsync(string? invalid = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a deposit by listing one or more items for sale on the marketplace.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit</c>
    /// <para>
    /// It is recommended to chunk requests to a maximum of 20 items per call to avoid rate limiting.
    /// The <c>coin_value</c> is in coincents (1 coin = 100 coincents).
    /// Individual deposit states are announced via the WebSocket <c>deposit_failed</c> event.
    /// </para>
    /// </remarks>
    /// <param name="request">The deposit request containing items to list.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DepositResponse"/> containing the tracking code for the deposit.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="CreateDepositRequest.Items"/> is empty or exceeds 20 items.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<DepositResponse> CreateDepositAsync(CreateDepositRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks the processing status of a deposit using its tracking code.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/deposit/status/{trackingCode}</c>
    /// </remarks>
    /// <param name="trackingCode">The tracking code returned from <see cref="CreateDepositAsync"/>.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DepositStatusResponse"/> containing the current deposit status.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="trackingCode"/> is null or whitespace.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<DepositStatusResponse> CheckDepositStatusAsync(string trackingCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a single processing deposit that has received no bids.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{deposit_id}/cancel</c>
    /// <para>
    /// Once a bid has been placed on an item, it is no longer eligible for cancellation.
    /// </para>
    /// </remarks>
    /// <param name="depositId">The ID of the deposit to cancel.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task CancelDepositAsync(int depositId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels multiple processing deposits that have received no bids.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/cancel</c>
    /// <para>
    /// Once a bid has been placed on an item, it is no longer eligible for cancellation.
    /// </para>
    /// </remarks>
    /// <param name="request">The request containing the list of deposit IDs to cancel.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="CancelMultipleDepositsRequest.Ids"/> is empty.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task CancelMultipleDepositsAsync(CancelMultipleDepositsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Immediately sells a deposit that has at least one bid (auction sales only).
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/deposit/{deposit_id}/sell</c>
    /// <para>
    /// The deposit must have at least one bid placed on it. This only works for auction-style listings.
    /// </para>
    /// </remarks>
    /// <param name="depositId">The ID of the deposit to sell.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task SellNowAsync(int depositId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the listing price of a single deposit.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>PATCH /trading/deposit/{depositIdOrAssetId}</c>
    /// <para>
    /// You can identify the deposit by either its Empire deposit ID or the Steam asset ID.
    /// </para>
    /// </remarks>
    /// <param name="depositIdOrAssetId">The Empire deposit ID or Steam asset ID of the item.</param>
    /// <param name="request">The request containing the new price in coincents.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DepositResponse"/> with the updated deposit details.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositIdOrAssetId"/> is less than or equal to 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="UpdateListingPriceRequest.CoinValue"/> is not positive.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<DepositResponse> UpdateListingPriceAsync(long depositIdOrAssetId, UpdateListingPriceRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the listing prices for up to 20 deposits in a single request.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>PATCH /trading/deposit/bulk</c>
    /// </remarks>
    /// <param name="request">The request containing items and their new prices.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="BulkUpdateListingPricesRequest.Items"/> is empty or exceeds 20 items.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task BulkUpdateListingPricesAsync(BulkUpdateListingPricesRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the depositor's statistics for a specific deposit, including delivery rates and Steam level.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/deposit/{deposit_id}/stats</c>
    /// </remarks>
    /// <param name="depositId">The ID of the deposit to get stats for.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DepositorStatsResponse"/> containing the depositor's statistics.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depositId"/> is less than or equal to 0.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<DepositorStatsResponse> GetDepositorStatsAsync(int depositId, CancellationToken cancellationToken = default);
}
