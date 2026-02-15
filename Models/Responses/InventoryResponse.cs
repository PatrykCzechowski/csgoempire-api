using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the inventory endpoint containing the user's CS2 inventory items.
/// </summary>
public sealed class InventoryResponse
{
    /// <summary>
    /// The list of inventory items.
    /// </summary>
    [JsonPropertyName("data")]
    public List<InventoryItem> Data { get; set; } = [];
}
