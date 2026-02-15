using System.Text.Json.Serialization;
using CsGoEmpire.Api.Models.Common;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents an item from the user's CS2 Steam inventory.
/// </summary>
public sealed class InventoryItem
{
    /// <summary>
    /// The Steam asset ID of the item.
    /// </summary>
    [JsonPropertyName("asset_id")]
    public long AssetId { get; set; }

    /// <summary>
    /// The Empire item ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The Steam market name of the item.
    /// </summary>
    [JsonPropertyName("market_name")]
    public string MarketName { get; set; } = string.Empty;

    /// <summary>
    /// The market value of the item in coincents.
    /// </summary>
    [JsonPropertyName("market_value")]
    public int MarketValue { get; set; }

    /// <summary>
    /// The URL of the item's icon image.
    /// </summary>
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// Whether the item is a commodity (e.g., cases, common skins).
    /// </summary>
    [JsonPropertyName("is_commodity")]
    public bool IsCommodity { get; set; }

    /// <summary>
    /// The hex color code for the item name display.
    /// </summary>
    [JsonPropertyName("name_color")]
    public string NameColor { get; set; } = string.Empty;

    /// <summary>
    /// The item's float wear value (0–1 scale). Null for commodity items.
    /// </summary>
    [JsonPropertyName("wear")]
    public double? Wear { get; set; }

    /// <summary>
    /// The preview ID for the item's 3D preview. Null if not available.
    /// </summary>
    [JsonPropertyName("preview_id")]
    public string? PreviewId { get; set; }

    /// <summary>
    /// The stickers applied to the item.
    /// </summary>
    [JsonPropertyName("stickers")]
    public List<StickerInfo>? Stickers { get; set; }

    /// <summary>
    /// Whether the item's price is considered unreliable by the platform.
    /// </summary>
    [JsonPropertyName("price_is_unreliable")]
    public bool PriceIsUnreliable { get; set; }

    /// <summary>
    /// The suggested listing price in coincents.
    /// </summary>
    [JsonPropertyName("suggested_price")]
    public int SuggestedPrice { get; set; }

    /// <summary>
    /// Search/filter metadata for the item.
    /// </summary>
    [JsonPropertyName("item_search")]
    public ItemSearchInfo? ItemSearch { get; set; }

    /// <summary>
    /// The item type description (e.g., "Mil-Spec Grade Sniper Rifle").
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
