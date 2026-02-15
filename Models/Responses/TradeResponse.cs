using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response containing a single trade's data.
/// </summary>
public sealed class TradeResponse
{
    /// <summary>
    /// The trade data.
    /// </summary>
    [JsonPropertyName("data")]
    public TradeData? Data { get; set; }
}
