using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Represents a single item in a bulk price update request.
/// </summary>
public sealed class BulkUpdateItem
{
    /// <summary>
    /// The deposit ID of the item to update.
    /// </summary>
    [JsonPropertyName("deposit_id")]
    public int DepositId { get; set; }

    /// <summary>
    /// The new price in coincents (1 coin = 100 coincents).
    /// </summary>
    [JsonPropertyName("coin_value")]
    public int CoinValue { get; set; }
}
