using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for sending a tip to another user.
/// </summary>
public sealed class TipRequest
{
    /// <summary>
    /// The Empire user ID of the recipient. Mutually exclusive with <see cref="SteamId"/>.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    /// <summary>
    /// The Steam ID 64 of the recipient. Mutually exclusive with <see cref="UserId"/>.
    /// </summary>
    [JsonPropertyName("steam_id")]
    public string? SteamId { get; set; }

    /// <summary>
    /// The tip amount in coincents (1 coin = 100 coincents).
    /// </summary>
    [JsonPropertyName("amount")]
    public string Amount { get; set; } = string.Empty;
}
