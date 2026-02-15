using System.Text.Json.Serialization;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Models.WebSocket;

/// <summary>
/// Represents a trade status update event received via WebSocket.
/// </summary>
public sealed class TradeStatusEvent
{
    /// <summary>
    /// The type of trade ("withdrawal", "deposit", "bid").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The trade data containing status, item, metadata, etc.
    /// </summary>
    [JsonPropertyName("data")]
    public TradeData Data { get; set; } = null!;
}
