using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents the data of a single trade (deposit, withdrawal, or bid).
/// </summary>
public sealed class TradeData
{
    /// <summary>
    /// The trade/deposit ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The numeric status code of the trade. Maps to <see cref="Enums.TradeStatus"/>.
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// A human-readable status message.
    /// </summary>
    [JsonPropertyName("status_message")]
    public string? StatusMessage { get; set; }

    /// <summary>
    /// The item ID associated with the trade.
    /// </summary>
    [JsonPropertyName("item_id")]
    public long ItemId { get; set; }

    /// <summary>
    /// The Steam trade offer ID.
    /// </summary>
    [JsonPropertyName("tradeoffer_id")]
    public long TradeofferId { get; set; }

    /// <summary>
    /// The total value of the trade in coincents.
    /// </summary>
    [JsonPropertyName("total_value")]
    public int TotalValue { get; set; }

    /// <summary>
    /// ISO 8601 / datetime timestamp when the trade was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// ISO 8601 / datetime timestamp when the trade was last updated.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    /// <summary>
    /// Additional metadata for the trade (validation, expiry, partner).
    /// </summary>
    [JsonPropertyName("metadata")]
    public TradeMetadata? Metadata { get; set; }

    /// <summary>
    /// The item involved in the trade.
    /// </summary>
    [JsonPropertyName("item")]
    public MarketItem? Item { get; set; }
}
