using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Paginated response containing listed marketplace items.
/// </summary>
public sealed class ListedItemsResponse
{
    /// <summary>
    /// The current page number.
    /// </summary>
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// The list of marketplace items on this page.
    /// </summary>
    [JsonPropertyName("data")]
    public List<MarketItem> Data { get; set; } = [];

    /// <summary>
    /// The last page number.
    /// </summary>
    [JsonPropertyName("last_page")]
    public int LastPage { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }
}
