using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Common;

/// <summary>
/// Represents search/filter metadata for a CS2 item.
/// </summary>
public sealed class ItemSearchInfo
{
    /// <summary>
    /// The item category (e.g., "Rifle", "Pistol").
    /// </summary>
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The item type (e.g., "Weapon", "Gloves").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The item sub-type (e.g., "AK-47", "M4A4").
    /// </summary>
    [JsonPropertyName("sub_type")]
    public string SubType { get; set; } = string.Empty;

    /// <summary>
    /// The rarity of the item (e.g., "Covert", "Classified").
    /// </summary>
    [JsonPropertyName("rarity")]
    public string Rarity { get; set; } = string.Empty;
}
