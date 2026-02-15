using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents a blocked user entry.
/// </summary>
public sealed class BlockedUser
{
    /// <summary>
    /// The blocked user's Steam ID 64.
    /// </summary>
    [JsonPropertyName("steam_id")]
    public string SteamId { get; set; } = string.Empty;

    /// <summary>
    /// The blocked user's Steam display name.
    /// </summary>
    [JsonPropertyName("steam_name")]
    public string? SteamName { get; set; }

    /// <summary>
    /// URL of the blocked user's avatar.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    /// <summary>
    /// ISO 8601 / datetime timestamp when the user was blocked.
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
}
