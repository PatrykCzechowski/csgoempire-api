using System.Text.Json.Serialization;
using CsGoEmpire.Api.Models.Common;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the depositor stats endpoint.
/// </summary>
public sealed class DepositorStatsResponse
{
    /// <summary>
    /// The depositor's statistics.
    /// </summary>
    [JsonPropertyName("data")]
    public DepositorStats? Data { get; set; }
}
