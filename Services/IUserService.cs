using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire user-related endpoints including settings, tipping, and transaction history.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Updates the authenticated user's trading settings such as trade URL or privacy protection level.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/user/settings</c>
    /// <para>
    /// At least one of <see cref="UpdateSettingsRequest.TradeUrl"/> or
    /// <see cref="UpdateSettingsRequest.MarketplacePrivacyProtectionLevel"/> should be provided.
    /// </para>
    /// </remarks>
    /// <param name="request">The settings to update.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="SettingsResponse"/> containing the updated settings and confirmation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<SettingsResponse> UpdateSettingsAsync(UpdateSettingsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a tip to another user identified by their Empire user ID or Steam ID 64.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /user/tip</c>
    /// <para>
    /// Exactly one of <see cref="TipRequest.UserId"/> or <see cref="TipRequest.SteamId"/> must be provided.
    /// The amount is specified in coincents (1 coin = 100 coincents).
    /// </para>
    /// </remarks>
    /// <param name="request">The tip details including recipient and amount.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="TipResponse"/> containing the result of the tip operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when neither <see cref="TipRequest.UserId"/> nor <see cref="TipRequest.SteamId"/> is provided, or when <see cref="TipRequest.Amount"/> is empty.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<TipResponse> SendTipAsync(TipRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the authenticated user's transaction history with pagination.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /user/transactions</c>
    /// </remarks>
    /// <param name="page">The page number to retrieve. Pass <c>null</c> to use the API default (page 1).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="TransactionHistoryResponse"/> containing the paginated transaction list.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="page"/> is less than 1.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<TransactionHistoryResponse> GetTransactionHistoryAsync(int? page = null, CancellationToken cancellationToken = default);
}
