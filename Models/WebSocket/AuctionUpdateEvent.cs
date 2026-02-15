using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Models.WebSocket;

/// <summary>
/// Represents an auction update event received via WebSocket when someone places a bid.
/// </summary>
public sealed class AuctionUpdateEvent
{
    /// <summary>
    /// The deposit/item ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Percentage above (positive) or below (negative) the recommended price.
    /// </summary>
    [JsonPropertyName("above_recommended_price")]
    public double AboveRecommendedPrice { get; set; }

    /// <summary>
    /// The highest bid amount in coincents.
    /// </summary>
    [JsonPropertyName("auction_highest_bid")]
    public int AuctionHighestBid { get; set; }

    /// <summary>
    /// The user ID of the highest bidder.
    /// </summary>
    [JsonPropertyName("auction_highest_bidder")]
    public int AuctionHighestBidder { get; set; }

    /// <summary>
    /// The total number of bids placed.
    /// </summary>
    [JsonPropertyName("auction_number_of_bids")]
    public int AuctionNumberOfBids { get; set; }

    /// <summary>
    /// Unix timestamp when the auction ends.
    /// </summary>
    [JsonPropertyName("auction_ends_at")]
    public long AuctionEndsAt { get; set; }
}
