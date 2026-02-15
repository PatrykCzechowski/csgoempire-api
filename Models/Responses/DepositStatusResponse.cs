using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Responses;

/// <summary>
/// Response from the deposit status check endpoint.
/// </summary>
public sealed class DepositStatusResponse
{
    /// <summary>
    /// The deposit data or status information.
    /// </summary>
    [JsonPropertyName("data")]
    public DepositStatusData? Data { get; set; }
}

/// <summary>
/// Deposit status data within a status check response.
/// </summary>
public sealed class DepositStatusData
{
    /// <summary>
    /// The deposit ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The current status of the deposit.
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// A human-readable status message.
    /// </summary>
    [JsonPropertyName("status_message")]
    public string? StatusMessage { get; set; }

    /// <summary>
    /// The item associated with the deposit.
    /// </summary>
    [JsonPropertyName("item")]
    public MarketItem? Item { get; set; }
}
