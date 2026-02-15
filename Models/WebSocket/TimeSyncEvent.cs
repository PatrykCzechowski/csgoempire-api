using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.WebSocket;

/// <summary>
/// Represents a time synchronization event received via WebSocket.
/// Contains the server's current timestamp for clock synchronization.
/// </summary>
public sealed class TimeSyncEvent
{
    /// <summary>
    /// The server's current Unix timestamp in milliseconds.
    /// </summary>
    [JsonPropertyName("server_timestamp")]
    public long ServerTimestamp { get; set; }
}
