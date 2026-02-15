using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.Requests;

/// <summary>
/// Request body for creating a withdrawal (purchasing an item).
/// </summary>
public sealed class CreateWithdrawalRequest
{
    /// <summary>
    /// The price in coincents to offer for the item. Optional for direct withdrawals.
    /// </summary>
    [JsonPropertyName("coin_value")]
    public int? CoinValue { get; set; }
}
