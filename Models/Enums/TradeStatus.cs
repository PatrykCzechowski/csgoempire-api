namespace CsGoEmpire.Api.Models.Enums;

/// <summary>
/// Represents the status of a trade on CSGOEmpire.
/// </summary>
public enum TradeStatus
{
    /// <summary>An error occurred during the trade.</summary>
    Error = -1,

    /// <summary>The trade is pending.</summary>
    Pending = 0,

    /// <summary>The trade has been received.</summary>
    Received = 1,

    /// <summary>The trade is being processed.</summary>
    Processing = 2,

    /// <summary>The trade is being sent.</summary>
    Sending = 3,

    /// <summary>The trade is awaiting confirmation.</summary>
    Confirming = 4,

    /// <summary>The trade has been sent.</summary>
    Sent = 5,

    /// <summary>The trade has been completed successfully.</summary>
    Completed = 6,

    /// <summary>The trade was declined.</summary>
    Declined = 7,

    /// <summary>The trade was canceled.</summary>
    Canceled = 8,

    /// <summary>The trade has timed out.</summary>
    TimedOut = 9,

    /// <summary>The trade value has been credited.</summary>
    Credited = 10,

    /// <summary>The trade is under dispute.</summary>
    Disputed = 11,

    /// <summary>The trade is completed but may be reversed.</summary>
    CompletedButReversible = 13,

    /// <summary>The trade has been reversed.</summary>
    TradeReversed = 14,

    /// <summary>The trade timed out but may be reversed.</summary>
    TimedOutButReversible = 15
}
