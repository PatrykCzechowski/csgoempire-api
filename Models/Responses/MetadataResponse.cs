using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the metadata/socket endpoint containing user profile and WebSocket authentication data.
/// </summary>
public sealed class MetadataResponse
{
    /// <summary>
    /// The authenticated user's profile.
    /// </summary>
    [JsonPropertyName("user")]
    public UserProfile User { get; set; } = null!;

    /// <summary>
    /// The socket authentication token for WebSocket connections.
    /// </summary>
    [JsonPropertyName("socket_token")]
    public string SocketToken { get; set; } = string.Empty;

    /// <summary>
    /// The signature to use when authenticating with the WebSocket.
    /// </summary>
    [JsonPropertyName("socket_signature")]
    public string SocketSignature { get; set; } = string.Empty;
}
