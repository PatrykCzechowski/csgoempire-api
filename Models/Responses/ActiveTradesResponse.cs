using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response containing a list of active trades for the authenticated user.
/// </summary>
public sealed class ActiveTradesResponse
{
    /// <summary>
    /// The list of active trades.
    /// </summary>
    [JsonPropertyName("data")]
    public List<TradeData> Data { get; set; } = [];
}
