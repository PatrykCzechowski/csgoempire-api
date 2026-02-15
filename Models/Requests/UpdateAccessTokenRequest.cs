using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for updating (or setting) the Steam access token for trade automation.
/// </summary>
public sealed class UpdateAccessTokenRequest
{
    /// <summary>
    /// A valid Steam access token. The token expires every 24 hours.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}
