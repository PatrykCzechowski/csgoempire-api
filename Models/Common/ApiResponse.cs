using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Common;

/// <summary>
/// A generic wrapper for CSGOEmpire API responses.
/// </summary>
/// <typeparam name="T">The type of the response data.</typeparam>
public sealed class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the API request was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// The response data payload.
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// An optional message from the API, typically present on errors.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
