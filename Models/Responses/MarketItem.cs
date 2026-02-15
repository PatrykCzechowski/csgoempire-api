using System.Text.Json.Serialization;
using CsGoEmpire.Api.Models.Common;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents an item listed on the CSGOEmpire marketplace.
/// Extends inventory item data with marketplace-specific fields (auctions, pricing, depositor stats).
/// </summary>
public sealed class MarketItem
{
    /// <summary>
    /// The Empire item/deposit ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The Steam asset ID of the item.
    /// </summary>
    [JsonPropertyName("asset_id")]
    public long? AssetId { get; set; }

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
    /// Whether the item is a commodity.
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
    /// The preview ID for 3D preview. Null if not available.
    /// </summary>
    [JsonPropertyName("preview_id")]
    public string? PreviewId { get; set; }

    /// <summary>
    /// Whether the item's price is considered unreliable.
    /// </summary>
    [JsonPropertyName("price_is_unreliable")]
    public bool PriceIsUnreliable { get; set; }

    /// <summary>
    /// The suggested listing price in coincents.
    /// </summary>
    [JsonPropertyName("suggested_price")]
    public int SuggestedPrice { get; set; }

    /// <summary>
    /// The stickers applied to the item.
    /// </summary>
    [JsonPropertyName("stickers")]
    public List<StickerInfo>? Stickers { get; set; }

    /// <summary>
    /// Search/filter metadata for the item.
    /// </summary>
    [JsonPropertyName("item_search")]
    public ItemSearchInfo? ItemSearch { get; set; }

    /// <summary>
    /// The item type description.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    // ---- Marketplace-specific fields ----

    /// <summary>
    /// Unix timestamp when the auction ends. Null for non-auction items.
    /// </summary>
    [JsonPropertyName("auction_ends_at")]
    public long? AuctionEndsAt { get; set; }

    /// <summary>
    /// The highest bid amount in coincents. Null if no bids.
    /// </summary>
    [JsonPropertyName("auction_highest_bid")]
    public int? AuctionHighestBid { get; set; }

    /// <summary>
    /// The user ID of the highest bidder. Null if no bids.
    /// </summary>
    [JsonPropertyName("auction_highest_bidder")]
    public int? AuctionHighestBidder { get; set; }

    /// <summary>
    /// The total number of bids placed on the item.
    /// </summary>
    [JsonPropertyName("auction_number_of_bids")]
    public int AuctionNumberOfBids { get; set; }

    /// <summary>
    /// ISO 8601 timestamp when the item was published to the marketplace.
    /// </summary>
    [JsonPropertyName("published_at")]
    public string? PublishedAt { get; set; }

    /// <summary>
    /// The marketplace privacy protection level ("base" or "strict").
    /// </summary>
    [JsonPropertyName("marketplace_privacy_protection_level")]
    public string? MarketplacePrivacyProtectionLevel { get; set; }

    /// <summary>
    /// Percentage above (positive) or below (negative) the recommended price.
    /// </summary>
    [JsonPropertyName("above_recommended_price")]
    public double AboveRecommendedPrice { get; set; }

    /// <summary>
    /// The listed purchase price in coincents.
    /// </summary>
    [JsonPropertyName("purchase_price")]
    public int PurchasePrice { get; set; }

    /// <summary>
    /// Blue gem percentage for applicable knives. Null for non-applicable items.
    /// </summary>
    [JsonPropertyName("blue_percentage")]
    public double? BluePercentage { get; set; }

    /// <summary>
    /// Fade percentage for applicable items. Null for non-applicable items.
    /// </summary>
    [JsonPropertyName("fade_percentage")]
    public double? FadePercentage { get; set; }

    /// <summary>
    /// The wear condition name (e.g., "Factory New", "Field-Tested").
    /// </summary>
    [JsonPropertyName("wear_name")]
    public string? WearName { get; set; }

    /// <summary>
    /// Statistics about the depositor's trade history and reliability.
    /// </summary>
    [JsonPropertyName("depositor_stats")]
    public DepositorStats? DepositorStats { get; set; }
}
