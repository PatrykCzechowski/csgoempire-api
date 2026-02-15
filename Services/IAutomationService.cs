using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire trade automation endpoints for managing
/// Steam access tokens and triggering automated trade checks.
/// </summary>
public interface IAutomationService
{
    /// <summary>
    /// Retrieves the current status of the trade automation system, including access token expiry.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/automation/status</c>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>An <see cref="AutomationStatusResponse"/> containing the automation status.</returns>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<AutomationStatusResponse> GetStatusAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets or updates the Steam access token used for automated trade processing.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>PUT /trading/automation/access-token</c>
    /// <para>
    /// The Steam access token expires every 24 hours and must be refreshed regularly.
    /// Without a valid token, trade automation cannot process trades automatically.
    /// </para>
    /// </remarks>
    /// <param name="request">The request containing the new access token.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="UpdateAccessTokenRequest.AccessToken"/> is null or empty.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task UpdateAccessTokenAsync(UpdateAccessTokenRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the currently stored Steam access token, disabling trade automation.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>DELETE /trading/automation/access-token</c>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task DeleteAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Triggers a manual check of pending trades, processing any that require action.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/automation/check-trades</c>
    /// <para>
    /// This can be used as a fallback when WebSocket events are missed or as a periodic
    /// reconciliation step to ensure all trades are processed.
    /// </para>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task CheckTradesAsync(CancellationToken cancellationToken = default);
}
