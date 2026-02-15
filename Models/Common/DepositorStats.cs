using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Common;

/// <summary>
/// Represents statistics about a depositor's trade history and reliability.
/// </summary>
public sealed class DepositorStats
{
    /// <summary>
    /// The depositor's recent delivery rate as a percentage (0–1 scale).
    /// </summary>
    [JsonPropertyName("delivery_rate_recent")]
    public double? DeliveryRateRecent { get; set; }

    /// <summary>
    /// The depositor's long-term delivery rate as a percentage (0–1 scale).
    /// </summary>
    [JsonPropertyName("delivery_rate_long")]
    public double? DeliveryRateLong { get; set; }

    /// <summary>
    /// Average delivery time in minutes for recent trades.
    /// </summary>
    [JsonPropertyName("delivery_time_minutes_recent")]
    public int? DeliveryTimeMinutesRecent { get; set; }

    /// <summary>
    /// Average delivery time in minutes for long-term trades.
    /// </summary>
    [JsonPropertyName("delivery_time_minutes_long")]
    public int? DeliveryTimeMinutesLong { get; set; }

    /// <summary>
    /// A textual status of the delivery rate (e.g., "good", "average").
    /// </summary>
    [JsonPropertyName("delivery_rate_status")]
    public string? DeliveryRateStatus { get; set; }

    /// <summary>
    /// The minimum Steam level in the depositor's range.
    /// </summary>
    [JsonPropertyName("steam_level_min_range")]
    public int? SteamLevelMinRange { get; set; }

    /// <summary>
    /// The maximum Steam level in the depositor's range.
    /// </summary>
    [JsonPropertyName("steam_level_max_range")]
    public int? SteamLevelMaxRange { get; set; }

    /// <summary>
    /// Whether the user has trade notifications enabled.
    /// </summary>
    [JsonPropertyName("user_has_trade_notifications_enabled")]
    public bool UserHasTradeNotificationsEnabled { get; set; }

    /// <summary>
    /// The online status of the user.
    /// </summary>
    [JsonPropertyName("user_online_status")]
    public int UserOnlineStatus { get; set; }
}
