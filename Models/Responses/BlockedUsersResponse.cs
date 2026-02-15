using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the blocked users endpoint.
/// </summary>
public sealed class BlockedUsersResponse
{
    /// <summary>
    /// The list of blocked users.
    /// </summary>
    [JsonPropertyName("data")]
    public List<BlockedUser> Data { get; set; } = [];
}
