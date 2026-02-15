using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Represents item validation details within trade metadata.
/// </summary>
public sealed class ItemValidation
{
    /// <summary>
    /// Whether a valid item was detected in the trade.
    /// </summary>
    [JsonPropertyName("validItemDetected")]
    public bool ValidItemDetected { get; set; }

    /// <summary>
    /// Unix timestamp when the item was validated.
    /// </summary>
    [JsonPropertyName("validatedAt")]
    public long ValidatedAt { get; set; }
}
