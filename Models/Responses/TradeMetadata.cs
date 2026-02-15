using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Metadata associated with a trade, including item validation, expiry, and partner information.
/// </summary>
public sealed class TradeMetadata
{
    /// <summary>
    /// Item validation details for the trade.
    /// </summary>
    [JsonPropertyName("item_validation")]
    public ItemValidation? ItemValidation { get; set; }

    /// <summary>
    /// Unix timestamp when the trade expires.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public long? ExpiresAt { get; set; }

    /// <summary>
    /// Information about the trade partner.
    /// </summary>
    [JsonPropertyName("partner")]
    public TradePartner? Partner { get; set; }
}
