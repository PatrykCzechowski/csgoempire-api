using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Represents a single item to include in a deposit request.
/// Either <see cref="Id"/> (Empire item ID) or <see cref="AssetId"/> (Steam asset ID) must be provided.
/// </summary>
public sealed class DepositItem
{
    /// <summary>
    /// The Empire item ID. Mutually exclusive with <see cref="AssetId"/>.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// The Steam asset ID. Mutually exclusive with <see cref="Id"/>.
    /// </summary>
    [JsonPropertyName("asset_id")]
    public long? AssetId { get; set; }

    /// <summary>
    /// The price in coincents (1 coin = 100 coincents) at which to list the item.
    /// </summary>
    [JsonPropertyName("coin_value")]
    public int CoinValue { get; set; }
}
