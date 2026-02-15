using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for managing CS2 inventory and deposits on the CSGOEmpire marketplace.
/// </summary>
internal sealed class DepositService : IDepositService
{
    /// <summary>
    /// Maximum number of items allowed per single deposit or bulk update request.
    /// </summary>
    private const int MaxItemsPerRequest = 20;

    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<DepositService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DepositService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public DepositService(CsGoEmpireHttpClient httpClient, ILogger<DepositService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<InventoryResponse> GetInventoryAsync(
        string? invalid = null,
        CancellationToken cancellationToken = default)
    {
        var uri = string.IsNullOrWhiteSpace(invalid)
            ? "trading/user/inventory"
            : $"trading/user/inventory?invalid={Uri.EscapeDataString(invalid)}";

        _logger.LogInformation("Retrieving CS2 inventory (invalid filter: {Invalid})", invalid ?? "none");

        var response = await _httpClient
            .GetAsync<InventoryResponse>(uri, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Inventory retrieved — {Count} item(s)", response.Data?.Count ?? 0);

        return response;
    }

    /// <inheritdoc />
    public async Task<DepositResponse> CreateDepositAsync(
        CreateDepositRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Items is not { Count: > 0 })
        {
            throw new ArgumentException("At least one item must be provided.", nameof(request));
        }

        if (request.Items.Count > MaxItemsPerRequest)
        {
            throw new ArgumentException(
                $"A maximum of {MaxItemsPerRequest} items can be deposited per request. " +
                $"Received {request.Items.Count} items. Consider chunking into smaller batches.",
                nameof(request));
        }

        foreach (var item in request.Items)
        {
            if (item.Id is null && item.AssetId is null)
            {
                throw new ArgumentException(
                    "Each deposit item must have either an Id or AssetId.",
                    nameof(request));
            }

            if (item.CoinValue <= 0)
            {
                throw new ArgumentException(
                    $"CoinValue must be positive. Item (Id={item.Id}, AssetId={item.AssetId}) has CoinValue={item.CoinValue}.",
                    nameof(request));
            }
        }

        _logger.LogInformation("Creating deposit with {Count} item(s)", request.Items.Count);

        var response = await _httpClient
            .PostAsync<DepositResponse>("trading/deposit", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Deposit created — tracking code: {TrackingCode}", response.TrackingCode);

        return response;
    }

    /// <inheritdoc />
    public async Task<DepositStatusResponse> CheckDepositStatusAsync(
        string trackingCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(trackingCode))
        {
            throw new ArgumentException("Tracking code must not be null or empty.", nameof(trackingCode));
        }

        _logger.LogInformation("Checking deposit status for tracking code: {TrackingCode}", trackingCode);

        var response = await _httpClient
            .GetAsync<DepositStatusResponse>(
                $"trading/deposit/status/{Uri.EscapeDataString(trackingCode)}",
                cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Deposit status retrieved — ID: {Id}, Status: {Status}",
            response.Data?.Id, response.Data?.StatusMessage);

        return response;
    }

    /// <inheritdoc />
    public async Task CancelDepositAsync(
        int depositId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);

        _logger.LogInformation("Canceling deposit {DepositId}", depositId);

        await _httpClient
            .PostAsync($"trading/deposit/{depositId}/cancel", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Deposit {DepositId} canceled successfully", depositId);
    }

    /// <inheritdoc />
    public async Task CancelMultipleDepositsAsync(
        CancelMultipleDepositsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Ids is not { Count: > 0 })
        {
            throw new ArgumentException("At least one deposit ID must be provided.", nameof(request));
        }

        _logger.LogInformation("Canceling {Count} deposit(s)", request.Ids.Count);

        await _httpClient
            .PostAsync("trading/deposit/cancel", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("{Count} deposit(s) canceled successfully", request.Ids.Count);
    }

    /// <inheritdoc />
    public async Task SellNowAsync(
        int depositId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);

        _logger.LogInformation("Selling deposit {DepositId} immediately", depositId);

        await _httpClient
            .PostAsync($"trading/deposit/{depositId}/sell", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Deposit {DepositId} sold successfully", depositId);
    }

    /// <inheritdoc />
    public async Task<DepositResponse> UpdateListingPriceAsync(
        long depositIdOrAssetId,
        UpdateListingPriceRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositIdOrAssetId);
        ArgumentNullException.ThrowIfNull(request);

        if (request.CoinValue <= 0)
        {
            throw new ArgumentException("CoinValue must be a positive value.", nameof(request));
        }

        _logger.LogInformation(
            "Updating listing price for deposit/asset {Id} to {CoinValue} coincents",
            depositIdOrAssetId, request.CoinValue);

        var response = await _httpClient
            .PatchAsync<DepositResponse>($"trading/deposit/{depositIdOrAssetId}", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Listing price updated for deposit/asset {Id}", depositIdOrAssetId);

        return response;
    }

    /// <inheritdoc />
    public async Task BulkUpdateListingPricesAsync(
        BulkUpdateListingPricesRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Items is not { Count: > 0 })
        {
            throw new ArgumentException("At least one item must be provided.", nameof(request));
        }

        if (request.Items.Count > MaxItemsPerRequest)
        {
            throw new ArgumentException(
                $"A maximum of {MaxItemsPerRequest} items can be updated per request. " +
                $"Received {request.Items.Count} items.",
                nameof(request));
        }

        foreach (var item in request.Items)
        {
            if (item.DepositId <= 0)
            {
                throw new ArgumentException(
                    $"DepositId must be positive. Found DepositId={item.DepositId}.",
                    nameof(request));
            }

            if (item.CoinValue <= 0)
            {
                throw new ArgumentException(
                    $"CoinValue must be positive. Item DepositId={item.DepositId} has CoinValue={item.CoinValue}.",
                    nameof(request));
            }
        }

        _logger.LogInformation("Bulk updating listing prices for {Count} item(s)", request.Items.Count);

        await _httpClient
            .PatchAsync("trading/deposit/bulk", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("{Count} listing price(s) updated successfully", request.Items.Count);
    }

    /// <inheritdoc />
    public async Task<DepositorStatsResponse> GetDepositorStatsAsync(
        int depositId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depositId);

        _logger.LogInformation("Retrieving depositor stats for deposit {DepositId}", depositId);

        var response = await _httpClient
            .GetAsync<DepositorStatsResponse>($"trading/deposit/{depositId}/stats", cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Depositor stats retrieved for deposit {DepositId}", depositId);

        return response;
    }
}
