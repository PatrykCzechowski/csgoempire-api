using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Common;

/// <summary>
/// Represents sticker information on a CS2 item.
/// </summary>
public sealed class StickerInfo
{
    /// <summary>
    /// The unique identifier of the sticker.
    /// </summary>
    [JsonPropertyName("sticker_id")]
    public int? StickerId { get; set; }

    /// <summary>
    /// The wear value of the sticker (0–1 scale).
    /// </summary>
    [JsonPropertyName("wear")]
    public double? Wear { get; set; }

    /// <summary>
    /// The display name of the sticker.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the sticker image.
    /// </summary>
    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;
}
