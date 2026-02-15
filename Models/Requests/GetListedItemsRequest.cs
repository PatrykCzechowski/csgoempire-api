using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Query parameters for fetching listed items from the marketplace.
/// </summary>
public sealed class GetListedItemsRequest
{
    /// <summary>
    /// Number of items per page. Min 1, max 200 (guests) or 2500 (authenticated). Defaults to 10.
    /// </summary>
    [JsonPropertyName("per_page")]
    public int PerPage { get; set; } = 10;

    /// <summary>
    /// Page number to fetch. Defaults to 1.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Filter for auction-only items. Use "yes" or "no".
    /// </summary>
    [JsonPropertyName("auction")]
    public string? Auction { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc". Defaults to "asc".
    /// </summary>
    [JsonPropertyName("sort")]
    public string? Sort { get; set; }

    /// <summary>
    /// Item market name to search. Minimum 2 characters.
    /// </summary>
    [JsonPropertyName("search")]
    public string? Search { get; set; }

    /// <summary>
    /// Sorting order field. Supported: "market_value".
    /// </summary>
    [JsonPropertyName("order")]
    public string? Order { get; set; }

    /// <summary>
    /// Minimum item price in coincents.
    /// </summary>
    [JsonPropertyName("price_min")]
    public int? PriceMin { get; set; }

    /// <summary>
    /// Maximum item price in coincents.
    /// </summary>
    [JsonPropertyName("price_max")]
    public int? PriceMax { get; set; }

    /// <summary>
    /// Maximum item percentage above recommended price to show.
    /// </summary>
    [JsonPropertyName("price_max_above")]
    public int? PriceMaxAbove { get; set; }

    /// <summary>
    /// Minimum delivery time average (in minutes) for the last 100 items.
    /// </summary>
    [JsonPropertyName("delivery_time_long_min")]
    public int? DeliveryTimeLongMin { get; set; }

    /// <summary>
    /// Maximum delivery time average (in minutes) for the last 100 items.
    /// </summary>
    [JsonPropertyName("delivery_time_long_max")]
    public int? DeliveryTimeLongMax { get; set; }

    /// <summary>
    /// Minimum float wear value (0–1).
    /// </summary>
    [JsonPropertyName("wear_min")]
    public string? WearMin { get; set; }

    /// <summary>
    /// Maximum float wear value (0–1).
    /// </summary>
    [JsonPropertyName("wear_max")]
    public string? WearMax { get; set; }

    /// <summary>
    /// Filter for items with stickers. Use "yes" or "no".
    /// </summary>
    [JsonPropertyName("has_stickers")]
    public string? HasStickers { get; set; }

    /// <summary>
    /// Filter for commodity items. Use "yes" or "no".
    /// Cannot be combined with wear/sticker filters.
    /// </summary>
    [JsonPropertyName("is_commodity")]
    public string? IsCommodity { get; set; }

    /// <summary>
    /// Builds a query string from all non-null properties.
    /// </summary>
    internal string ToQueryString()
    {
        var parameters = new List<string>
        {
            $"per_page={PerPage}",
            $"page={Page}"
        };

        if (Auction is not null) parameters.Add($"auction={Uri.EscapeDataString(Auction)}");
        if (Sort is not null) parameters.Add($"sort={Uri.EscapeDataString(Sort)}");
        if (Search is not null) parameters.Add($"search={Uri.EscapeDataString(Search)}");
        if (Order is not null) parameters.Add($"order={Uri.EscapeDataString(Order)}");
        if (PriceMin.HasValue) parameters.Add($"price_min={PriceMin.Value}");
        if (PriceMax.HasValue) parameters.Add($"price_max={PriceMax.Value}");
        if (PriceMaxAbove.HasValue) parameters.Add($"price_max_above={PriceMaxAbove.Value}");
        if (DeliveryTimeLongMin.HasValue) parameters.Add($"delivery_time_long_min={DeliveryTimeLongMin.Value}");
        if (DeliveryTimeLongMax.HasValue) parameters.Add($"delivery_time_long_max={DeliveryTimeLongMax.Value}");
        if (WearMin is not null) parameters.Add($"wear_min={Uri.EscapeDataString(WearMin)}");
        if (WearMax is not null) parameters.Add($"wear_max={Uri.EscapeDataString(WearMax)}");
        if (HasStickers is not null) parameters.Add($"has_stickers={Uri.EscapeDataString(HasStickers)}");
        if (IsCommodity is not null) parameters.Add($"is_commodity={Uri.EscapeDataString(IsCommodity)}");

        return string.Join("&", parameters);
    }
}
