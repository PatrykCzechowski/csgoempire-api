using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the active auctions endpoint.
/// </summary>
public sealed class AuctionsResponse
{
    /// <summary>
    /// The list of active auction items.
    /// </summary>
    [JsonPropertyName("data")]
    public List<MarketItem> Data { get; set; } = [];
}
