using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Models.Requests;
using CsGoEmpire.Api.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Services;

/// <summary>
/// Service for accessing CSGOEmpire user-related endpoints.
/// </summary>
internal sealed class UserService : IUserService
{
    private readonly CsGoEmpireHttpClient _httpClient;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="httpClient">The CSGOEmpire HTTP client.</param>
    /// <param name="logger">The logger instance.</param>
    public UserService(CsGoEmpireHttpClient httpClient, ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<SettingsResponse> UpdateSettingsAsync(
        UpdateSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.TradeUrl) &&
            string.IsNullOrWhiteSpace(request.MarketplacePrivacyProtectionLevel))
        {
            throw new ArgumentException(
                "At least one of TradeUrl or MarketplacePrivacyProtectionLevel must be provided.",
                nameof(request));
        }

        _logger.LogInformation("Updating user settings");

        var response = await _httpClient
            .PostAsync<SettingsResponse>("trading/user/settings", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("User settings updated successfully");

        return response;
    }

    /// <inheritdoc />
    public async Task<TipResponse> SendTipAsync(
        TipRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.UserId) && string.IsNullOrWhiteSpace(request.SteamId))
        {
            throw new ArgumentException(
                "Either UserId or SteamId must be provided.",
                nameof(request));
        }

        if (!string.IsNullOrWhiteSpace(request.UserId) && !string.IsNullOrWhiteSpace(request.SteamId))
        {
            throw new ArgumentException(
                "UserId and SteamId are mutually exclusive — provide only one.",
                nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Amount))
        {
            throw new ArgumentException(
                "Amount is required and must not be empty.",
                nameof(request));
        }

        if (!long.TryParse(request.Amount, out var amountValue) || amountValue <= 0)
        {
            throw new ArgumentException(
                "Amount must be a positive numeric value in coincents.",
                nameof(request));
        }

        _logger.LogInformation(
            "Sending tip of {Amount} coincents to {Recipient}",
            request.Amount,
            request.UserId ?? request.SteamId);

        var response = await _httpClient
            .PostAsync<TipResponse>("user/tip", request, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Tip sent successfully");

        return response;
    }

    /// <inheritdoc />
    public async Task<TransactionHistoryResponse> GetTransactionHistoryAsync(
        int? page = null,
        CancellationToken cancellationToken = default)
    {
        if (page is < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page number must be at least 1.");
        }

        var uri = page.HasValue
            ? $"user/transactions?page={page.Value}"
            : "user/transactions";

        _logger.LogInformation("Retrieving transaction history (page: {Page})", page?.ToString() ?? "default");

        var response = await _httpClient
            .GetAsync<TransactionHistoryResponse>(uri, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Transaction history retrieved — page {CurrentPage}/{LastPage}, {Count} transactions",
            response.CurrentPage, response.LastPage, response.Data?.Count ?? 0);

        return response;
    }
}
