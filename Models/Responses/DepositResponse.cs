using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response returned from the create deposit endpoint.
/// </summary>
public sealed class DepositResponse
{
    /// <summary>
    /// Whether the deposit request was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// A tracking code to check the deposit status.
    /// </summary>
    [JsonPropertyName("tracking_code")]
    public string? TrackingCode { get; set; }

    /// <summary>
    /// An optional message from the API.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
