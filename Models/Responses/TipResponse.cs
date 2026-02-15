using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the tipping endpoint.
/// </summary>
public sealed class TipResponse
{
    /// <summary>
    /// Whether the tip was sent successfully.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// An optional message from the API.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Data about the completed tip.
    /// </summary>
    [JsonPropertyName("data")]
    public TipData? Data { get; set; }
}

/// <summary>
/// Data about a completed tip transaction.
/// </summary>
public sealed class TipData
{
    /// <summary>
    /// The Empire user ID of the recipient.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The amount tipped in coincents.
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    /// <summary>
    /// The tip sender's user ID.
    /// </summary>
    [JsonPropertyName("sender_id")]
    public int SenderId { get; set; }
}
