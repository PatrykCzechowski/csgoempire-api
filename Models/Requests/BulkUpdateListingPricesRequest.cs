using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for updating prices of up to 20 deposits in a single request.
/// </summary>
public sealed class BulkUpdateListingPricesRequest
{
    /// <summary>
    /// The list of items with updated prices. Maximum 20 items per request.
    /// </summary>
    [JsonPropertyName("items")]
    public List<BulkUpdateItem> Items { get; set; } = [];
}
