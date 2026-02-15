using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents a single transaction in the user's transaction history.
/// </summary>
public sealed class Transaction
{
    /// <summary>
    /// The transaction ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The transaction key/type identifier.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// The transaction type description.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The amount of the transaction in coincents (positive for credit, negative for debit).
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    /// <summary>
    /// The resulting balance after the transaction in coincents.
    /// </summary>
    [JsonPropertyName("balance")]
    public long Balance { get; set; }

    /// <summary>
    /// ISO 8601 / datetime timestamp when the transaction occurred.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    /// <summary>
    /// ISO 8601 / datetime timestamp when the transaction was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// Additional metadata associated with the transaction.
    /// </summary>
    [JsonPropertyName("metadata")]
    public object? Metadata { get; set; }
}
