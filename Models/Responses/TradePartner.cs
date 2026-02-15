using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents the trade partner in a trade offer.
/// </summary>
public sealed class TradePartner
{
    /// <summary>
    /// The trade partner's Steam ID 64.
    /// </summary>
    [JsonPropertyName("steam_id")]
    public string SteamId { get; set; } = string.Empty;

    /// <summary>
    /// The trade partner's Steam display name.
    /// </summary>
    [JsonPropertyName("steam_name")]
    public string SteamName { get; set; } = string.Empty;

    /// <summary>
    /// URL of the trade partner's Steam avatar.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

    /// <summary>
    /// URL of the trade partner's full-size Steam avatar.
    /// </summary>
    [JsonPropertyName("avatar_full")]
    public string AvatarFull { get; set; } = string.Empty;

    /// <summary>
    /// URL of the trade partner's Steam profile.
    /// </summary>
    [JsonPropertyName("profile_url")]
    public string ProfileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Unix timestamp when the Steam account was created.
    /// </summary>
    [JsonPropertyName("timecreated")]
    public long Timecreated { get; set; }

    /// <summary>
    /// The trade partner's Steam community level.
    /// </summary>
    [JsonPropertyName("steam_level")]
    public int SteamLevel { get; set; }
}
