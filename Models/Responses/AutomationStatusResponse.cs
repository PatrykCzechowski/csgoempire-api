using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the automation status endpoint.
/// </summary>
public sealed class AutomationStatusResponse
{
    /// <summary>
    /// Whether the user has an active Steam access token set.
    /// </summary>
    [JsonPropertyName("has_access_token")]
    public bool HasAccessToken { get; set; }

    /// <summary>
    /// Unix timestamp when the access token expires. Null if no token is set.
    /// </summary>
    [JsonPropertyName("access_token_expires_at")]
    public long? AccessTokenExpiresAt { get; set; }
}
