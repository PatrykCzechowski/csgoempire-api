using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for placing a bid on an auction item.
/// </summary>
public sealed class PlaceBidRequest
{
    /// <summary>
    /// The bid value in coincents (1 coin = 100 coincents).
    /// </summary>
    [JsonPropertyName("bid_value")]
    public int BidValue { get; set; }
}
