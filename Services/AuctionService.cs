using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for accessing CSGOEmpire auction endpoints.
/// </summary>
internal sealed class AuctionService : IAuctionService
{
    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<AuctionService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public AuctionService(CsGoEmpireHttpClient httpClient, ILogger<AuctionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AuctionsResponse> GetActiveAuctionsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving active auctions");

        var response = await _httpClient
            .GetAsync<AuctionsResponse>("trading/user/auctions", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Active auctions retrieved — {Count} item(s)", response.Data?.Count ?? 0);

        return response;
    }

    /// <inheritdoc />
    public async Task PlaceBidAsync(
        int depositId,
        PlaceBidRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);
        ArgumentNullException.ThrowIfNull(request);

        if (request.BidValue <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request),
                request.BidValue,
                "BidValue must be a positive value (in coincents).");
        }

        _logger.LogInformation(
            "Placing bid of {BidValue} coincents on deposit {DepositId}",
            request.BidValue, depositId);

        await _httpClient
            .PostAsync($"trading/deposit/{depositId}/bid", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Bid of {BidValue} coincents placed successfully on deposit {DepositId}",
            request.BidValue, depositId);
    }
}
