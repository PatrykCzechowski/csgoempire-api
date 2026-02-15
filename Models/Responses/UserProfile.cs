using System.Text.Json.Serialization;
using CsGoEmpire.Api.Converters;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents the user profile data returned by the CSGOEmpire API.
/// </summary>
public sealed class UserProfile
{
    /// <summary>
    /// The Empire user ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user's Steam display name.
    /// </summary>
    [JsonPropertyName("steam_name")]
    public string SteamName { get; set; } = string.Empty;

    /// <summary>
    /// The user's Steam ID 64.
    /// </summary>
    [JsonPropertyName("steam_id")]
    public string SteamId { get; set; } = string.Empty;

    /// <summary>
    /// URL of the user's Steam avatar.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

    /// <summary>
    /// URL of the user's Steam profile.
    /// </summary>
    [JsonPropertyName("profile_url")]
    public string ProfileUrl { get; set; } = string.Empty;

    /// <summary>
    /// The user's current coin balance in coincents.
    /// </summary>
    [JsonPropertyName("balance")]
    public long Balance { get; set; }

    /// <summary>
    /// The user's Steam community level.
    /// </summary>
    [JsonPropertyName("steam_level")]
    public int SteamLevel { get; set; }

    /// <summary>
    /// Whether the user account is verified.
    /// </summary>
    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    /// <summary>
    /// Whether to hide the verified icon.
    /// </summary>
    [JsonPropertyName("hide_verified_icon")]
    public bool HideVerifiedIcon { get; set; }

    /// <summary>
    /// The user's assigned roles on the platform.
    /// </summary>
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// The short user identifier.
    /// </summary>
    [JsonPropertyName("uid")]
    public int Uid { get; set; }

    /// <summary>
    /// The user's display name on the platform.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The user's experience level on the platform.
    /// </summary>
    [JsonPropertyName("lvl")]
    public int Lvl { get; set; }

    /// <summary>
    /// Total amount bet by the user in coincents.
    /// </summary>
    [JsonPropertyName("total_bet")]
    public long TotalBet { get; set; }

    /// <summary>
    /// Total amount deposited by the user in coincents.
    /// </summary>
    [JsonPropertyName("total_deposit")]
    public long TotalDeposit { get; set; }

    /// <summary>
    /// The user's withdrawal limit in coincents.
    /// </summary>
    [JsonPropertyName("withdraw_limit")]
    public long WithdrawLimit { get; set; }

    /// <summary>
    /// Total profit/loss in coincents (can be negative).
    /// </summary>
    [JsonPropertyName("total_profit")]
    public long TotalProfit { get; set; }

    /// <summary>
    /// The user's referral code.
    /// </summary>
    [JsonPropertyName("referral_code")]
    public string? ReferralCode { get; set; }

    /// <summary>
    /// The referrer's user ID.
    /// </summary>
    [JsonPropertyName("ref_id")]
    public int RefId { get; set; }

    /// <summary>
    /// Bet threshold value.
    /// </summary>
    [JsonPropertyName("bet_threshold")]
    public int BetThreshold { get; set; }

    /// <summary>
    /// Unix timestamp until which the user is muted, or 0 if not muted.
    /// </summary>
    [JsonPropertyName("muted_until")]
    public long MutedUntil { get; set; }

    /// <summary>
    /// The reason for the user's mute, if applicable.
    /// </summary>
    [JsonPropertyName("mute_reason")]
    public string? MuteReason { get; set; }

    /// <summary>
    /// The UTM campaign associated with the user's registration.
    /// </summary>
    [JsonPropertyName("utm_campaign")]
    public string? UtmCampaign { get; set; }

    /// <summary>
    /// VAC ban status. 0 = not banned, other values indicate ban level.
    /// </summary>
    [JsonPropertyName("is_vac_banned")]
    public int IsVacBanned { get; set; }

    /// <summary>
    /// Whether the user is whitelisted.
    /// </summary>
    [JsonPropertyName("whitelisted")]
    [JsonConverter(typeof(BoolFromNumberConverter))]
    public bool Whitelisted { get; set; }

    /// <summary>
    /// The timestamp when the user registered.
    /// </summary>
    [JsonPropertyName("registration_timestamp")]
    public string? RegistrationTimestamp { get; set; }

    /// <summary>
    /// Whether the user has deposited before.
    /// </summary>
    [JsonPropertyName("deposited")]
    public bool Deposited { get; set; }

    /// <summary>
    /// Whether the user is a moderator.
    /// </summary>
    [JsonPropertyName("mod")]
    public bool Mod { get; set; }

    /// <summary>
    /// Whether the user is a super moderator.
    /// </summary>
    [JsonPropertyName("super_mod")]
    public bool SuperMod { get; set; }

    /// <summary>
    /// Whether the user is an admin.
    /// </summary>
    [JsonPropertyName("admin")]
    public bool Admin { get; set; }

    /// <summary>
    /// Whether the user is a helper mod.
    /// </summary>
    [JsonPropertyName("helper_mod")]
    public bool HelperMod { get; set; }

    /// <summary>
    /// Whether the user is in QA role.
    /// </summary>
    [JsonPropertyName("qa")]
    public bool Qa { get; set; }

    /// <summary>
    /// Custom chat tag, if any.
    /// </summary>
    [JsonPropertyName("chat_tag")]
    public string? ChatTag { get; set; }

    /// <summary>
    /// Badge text, if any.
    /// </summary>
    [JsonPropertyName("badge_text")]
    public string? BadgeText { get; set; }

    /// <summary>
    /// Localized badge text, if any.
    /// </summary>
    [JsonPropertyName("badge_text_localized")]
    public string? BadgeTextLocalized { get; set; }

    /// <summary>
    /// Badge color, if any.
    /// </summary>
    [JsonPropertyName("badge_color")]
    public string? BadgeColor { get; set; }

    /// <summary>
    /// Whether to hide the user's rank.
    /// </summary>
    [JsonPropertyName("hide_rank")]
    public bool? HideRank { get; set; }
}
