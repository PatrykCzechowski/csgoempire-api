using System.Text.Json.Serialization;
using CsGoEmpire.Api.Models.Responses;

namespace CsGoEmpire.Api.Models.WebSocket;

/// <summary>
/// Payload sent to the WebSocket to authenticate and identify the user.
/// </summary>
public sealed class IdentifyPayload
{
    /// <summary>
    /// The user's Empire user ID (from metadata).
    /// </summary>
    [JsonPropertyName("uid")]
    public int Uid { get; set; }

    /// <summary>
    /// The user's full profile model from metadata.
    /// </summary>
    [JsonPropertyName("model")]
    public UserProfile Model { get; set; } = null!;

    /// <summary>
    /// The socket authentication token (from metadata socket_token).
    /// </summary>
    [JsonPropertyName("authorizationToken")]
    public string AuthorizationToken { get; set; } = string.Empty;

    /// <summary>
    /// The socket signature (from metadata socket_signature / token_signature).
    /// </summary>
    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// An optional UUIDv4 device identifier.
    /// </summary>
    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}
