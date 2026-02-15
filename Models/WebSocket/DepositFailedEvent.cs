using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.WebSocket;

/// <summary>
/// Represents a deposit failed event received via WebSocket.
/// </summary>
public sealed class DepositFailedEvent
{
    /// <summary>
    /// The response object containing failure details.
    /// </summary>
    [JsonPropertyName("response")]
    public DepositFailedResponse Response { get; set; } = null!;
}

/// <summary>
/// The response wrapper within a deposit failed event.
/// </summary>
public sealed class DepositFailedResponse
{
    /// <summary>
    /// The error data payload.
    /// </summary>
    [JsonPropertyName("data")]
    public DepositFailedData Data { get; set; } = null!;

    /// <summary>
    /// The HTTP status code of the failed operation.
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// The HTTP status text (e.g., "Bad Request").
    /// </summary>
    [JsonPropertyName("statusText")]
    public string StatusText { get; set; } = string.Empty;
}

/// <summary>
/// Error data within a deposit failed response.
/// </summary>
public sealed class DepositFailedData
{
    /// <summary>
    /// Whether the operation was successful (always false for failures).
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// A human-readable error message describing the failure.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// A programmatic error key (e.g., "item_already_deposited").
    /// </summary>
    [JsonPropertyName("error_key")]
    public string ErrorKey { get; set; } = string.Empty;

    /// <summary>
    /// The item ID that failed to deposit.
    /// </summary>
    [JsonPropertyName("item_id")]
    public int ItemId { get; set; }
}
