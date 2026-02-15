using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Paginated response containing the user's transaction history.
/// </summary>
public sealed class TransactionHistoryResponse
{
    /// <summary>
    /// The current page number.
    /// </summary>
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// The list of transactions on this page.
    /// </summary>
    [JsonPropertyName("data")]
    public List<Transaction> Data { get; set; } = [];

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
    /// Total number of transactions across all pages.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }
}
