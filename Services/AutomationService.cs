using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for managing CSGOEmpire trade automation, including Steam access tokens
/// and automated trade checking.
/// </summary>
internal sealed class AutomationService : IAutomationService
{
    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<AutomationService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutomationService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public AutomationService(CsGoEmpireHttpClient httpClient, ILogger<AutomationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AutomationStatusResponse> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving automation status");

        var response = await _httpClient
            .GetAsync<AutomationStatusResponse>("trading/automation/status", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Automation status retrieved — hasToken: {HasToken}, expiresAt: {ExpiresAt}",
            response.HasAccessToken,
            response.AccessTokenExpiresAt?.ToString() ?? "N/A");

        return response;
    }

    /// <inheritdoc />
    public async Task UpdateAccessTokenAsync(
        UpdateAccessTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.AccessToken))
        {
            throw new ArgumentException(
                "Access token must not be null or empty.",
                nameof(request));
        }

        _logger.LogInformation("Updating Steam access token");

        await _httpClient
            .PutAsync("trading/automation/access-token", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Steam access token updated successfully");
    }

    /// <inheritdoc />
    public async Task DeleteAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting Steam access token");

        await _httpClient
            .DeleteAsync("trading/automation/access-token", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Steam access token deleted successfully");
    }

    /// <inheritdoc />
    public async Task CheckTradesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Triggering automated trade check");

        await _httpClient
            .PostAsync("trading/automation/check-trades", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Automated trade check completed successfully");
    }
}
