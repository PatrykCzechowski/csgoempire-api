using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.WebSocket;

/// <summary>
/// Payload sent to the WebSocket to set item filters (required after identification).
/// </summary>
public sealed class FiltersPayload
{
    /// <summary>
    /// Maximum item price filter. Set to a high value (e.g., 9999999) to receive all items.
    /// </summary>
    [JsonPropertyName("price_max")]
    public int PriceMax { get; set; } = 9999999;
}
