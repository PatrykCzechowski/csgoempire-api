using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for updating user trading settings.
/// </summary>
public sealed class UpdateSettingsRequest
{
    /// <summary>
    /// The full Steam trade URL to set.
    /// </summary>
    [JsonPropertyName("trade_url")]
    public string? TradeUrl { get; set; }

    /// <summary>
    /// The marketplace privacy protection level. Use "base" or "strict".
    /// </summary>
    [JsonPropertyName("marketplace_privacy_protection_level")]
    public string? MarketplacePrivacyProtectionLevel { get; set; }
}
