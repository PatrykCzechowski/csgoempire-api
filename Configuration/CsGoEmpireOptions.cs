namespace CsGoEmpire.Api.Configuration;

/// <summary>
/// Configuration options for the CSGOEmpire API client.
/// </summary>
public sealed class CsGoEmpireOptions
{
    /// <summary>
    /// The API key used for authentication. Required.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The base URL for the CSGOEmpire REST API.
    /// </summary>
    public string BaseUrl { get; set; } = "https://csgoempire.com/api/v2";

    /// <summary>
    /// The WebSocket URL for real-time trade updates.
    /// </summary>
    public string WebSocketUrl { get; set; } = "wss://trade.csgoempire.com/s/?EIO=3&transport=websocket";

    /// <summary>
    /// Maximum number of API requests allowed per minute. Defaults to 120.
    /// </summary>
    public int MaxRequestsPerMinute { get; set; } = 120;
}
