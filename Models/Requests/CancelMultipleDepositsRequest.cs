using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for canceling multiple deposits at once.
/// </summary>
public sealed class CancelMultipleDepositsRequest
{
    /// <summary>
    /// The list of deposit IDs to cancel.
    /// </summary>
    [JsonPropertyName("ids")]
    public List<int> Ids { get; set; } = [];
}
