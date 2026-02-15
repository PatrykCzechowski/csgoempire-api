using System.Text.RegularExpressions;
using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for managing the CSGOEmpire user block list.
/// </summary>
internal sealed partial class BlockListService : IBlockListService
{
    /// <summary>
    /// Regex pattern for validating Steam ID 64 format (17-digit numeric string starting with 7656).
    /// </summary>
    [GeneratedRegex(@"^7656\d{13}$", RegexOptions.Compiled)]
    private static partial Regex SteamId64Regex();

    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<BlockListService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockListService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public BlockListService(CsGoEmpireHttpClient httpClient, ILogger<BlockListService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task BlockUserAsync(string steamId, CancellationToken cancellationToken = default)
    {
        ValidateSteamId(steamId);

        _logger.LogInformation("Blocking user with Steam ID {SteamId}", steamId);

        await _httpClient
            .PostAsync($"trading/block-list/{steamId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("User {SteamId} blocked successfully", steamId);
    }

    /// <inheritdoc />
    public async Task UnblockUserAsync(string steamId, CancellationToken cancellationToken = default)
    {
        ValidateSteamId(steamId);

        _logger.LogInformation("Unblocking user with Steam ID {SteamId}", steamId);

        await _httpClient
            .DeleteAsync($"trading/block-list/{steamId}", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("User {SteamId} unblocked successfully", steamId);
    }

    /// <inheritdoc />
    public async Task<BlockedUsersResponse> GetBlockedUsersAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving blocked users list");

        var response = await _httpClient
            .GetAsync<BlockedUsersResponse>("trading/block-list", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Blocked users retrieved — {Count} user(s)", response.Data?.Count ?? 0);

        return response;
    }

    /// <summary>
    /// Validates that the provided string is a valid Steam ID 64.
    /// </summary>
    /// <param name="steamId">The Steam ID to validate.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="steamId"/> is null, empty, or not a valid Steam ID 64.</exception>
    private static void ValidateSteamId(string steamId)
    {
        if (string.IsNullOrWhiteSpace(steamId))
        {
            throw new ArgumentException("Steam ID must not be null or empty.", nameof(steamId));
        }

        if (!SteamId64Regex().IsMatch(steamId))
        {
            throw new ArgumentException(
                $"'{steamId}' is not a valid Steam ID 64. Expected a 17-digit numeric string starting with '7656'.",
                nameof(steamId));
        }
    }
}
