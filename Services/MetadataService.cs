using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for accessing CSGOEmpire metadata endpoints.
/// </summary>
internal sealed class MetadataService : IMetadataService
{
    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<MetadataService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public MetadataService(CsGoEmpireHttpClient httpClient, ILogger<MetadataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<MetadataResponse> GetSocketMetadataAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving socket metadata for authenticated user");

        var response = await _httpClient.GetAsync<MetadataResponse>("metadata/socket", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Socket metadata retrieved successfully for user {UserId}", response.User?.Id);

        return response;
    }
}
