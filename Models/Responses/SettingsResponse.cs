using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the update settings endpoint.
/// </summary>
public sealed class SettingsResponse
{
    /// <summary>
    /// Whether the settings update was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// The user's current trade URL.
    /// </summary>
    [JsonPropertyName("trade_url")]
    public string? TradeUrl { get; set; }

    /// <summary>
    /// The user's current marketplace privacy protection level.
    /// </summary>
    [JsonPropertyName("marketplace_privacy_protection_level")]
    public string? MarketplacePrivacyProtectionLevel { get; set; }

    /// <summary>
    /// An optional message from the API.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// The escrow seconds for the user's trade (Steam-side).
    /// </summary>
    [JsonPropertyName("escrow_seconds")]
    public int? EscrowSeconds { get; set; }
}
