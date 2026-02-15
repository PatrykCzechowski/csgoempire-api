using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for updating the listing price of a single deposit.
/// </summary>
public sealed class UpdateListingPriceRequest
{
    /// <summary>
    /// The new price in coincents (1 coin = 100 coincents).
    /// </summary>
    [JsonPropertyName("coin_value")]
    public int CoinValue { get; set; }
}
