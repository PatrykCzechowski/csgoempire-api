using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire metadata endpoints, including WebSocket authentication data.
/// </summary>
public interface IMetadataService
{
    /// <summary>
    /// Retrieves the authenticated user's profile data and WebSocket authentication credentials.
    /// </summary>
    /// <remarks>
    /// The returned <see cref="MetadataResponse"/> contains the socket token and signature required
    /// for WebSocket authentication. The token is valid for approximately 30 seconds.
    /// <para>
    /// <b>Endpoint:</b> <c>GET /metadata/socket</c>
    /// </para>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="MetadataResponse"/> containing the user profile and WebSocket credentials.</returns>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<MetadataResponse> GetSocketMetadataAsync(CancellationToken cancellationToken = default);
}
