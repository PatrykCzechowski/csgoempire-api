using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for creating one or more deposits (listing items for sale).
/// </summary>
public sealed class CreateDepositRequest
{
    /// <summary>
    /// The list of items to deposit. Maximum 20 items per request is recommended.
    /// </summary>
    [JsonPropertyName("items")]
    public List<DepositItem> Items { get; set; } = [];
}
