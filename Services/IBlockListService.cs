using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Provides access to CSGOEmpire user block list management endpoints.
/// </summary>
public interface IBlockListService
{
    /// <summary>
    /// Blocks a user by their Steam ID 64, preventing trades with them.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>POST /trading/block-list/{steam_id}</c>
    /// <para>
    /// Once blocked, the authenticated user will not receive trade offers from the specified user.
    /// </para>
    /// </remarks>
    /// <param name="steamId">The Steam ID 64 of the user to block.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="steamId"/> is null, empty, or not a valid Steam ID 64.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task BlockUserAsync(string steamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unblocks a previously blocked user by their Steam ID 64.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>DELETE /trading/block-list/{steam_id}</c>
    /// </remarks>
    /// <param name="steamId">The Steam ID 64 of the user to unblock.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="steamId"/> is null, empty, or not a valid Steam ID 64.</exception>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task UnblockUserAsync(string steamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the list of all users blocked by the authenticated user.
    /// </summary>
    /// <remarks>
    /// <b>Endpoint:</b> <c>GET /trading/block-list</c>
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="BlockedUsersResponse"/> containing the list of blocked users.</returns>
    /// <exception cref="Exceptions.CsGoEmpireApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="Exceptions.RateLimitExceededException">Thrown when the API rate limit is exceeded (HTTP 429).</exception>
    Task<BlockedUsersResponse> GetBlockedUsersAsync(CancellationToken cancellationToken = default);
}
