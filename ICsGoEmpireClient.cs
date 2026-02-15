using CsGoEmpire.Api.Services;
using CsGoEmpire.Api.WebSocket;

namespace CsGoEmpire.Api;

/// <summary>
/// The primary entry point for interacting with the CSGOEmpire API.
/// Aggregates all available services (REST and WebSocket) into a single, cohesive client interface.
/// </summary>
/// <remarks>
/// <para>
/// This interface follows the facade pattern, providing convenient access to all CSGOEmpire API
/// capabilities through strongly-typed service properties. Register the client via dependency injection
/// using <see cref="Extensions.ServiceCollectionExtensions.AddCsGoEmpireApi"/> or create an instance
/// directly through the <see cref="CsGoEmpireClient"/> implementation.
/// </para>
/// <para>
/// The client implements <see cref="IAsyncDisposable"/> to ensure the WebSocket connection is
/// properly cleaned up when the client is disposed.
/// </para>
/// <example>
/// <code>
/// // Via dependency injection
/// services.AddCsGoEmpireApi(options => options.ApiKey = "your-api-key");
///
/// // Usage
/// var metadata = await client.Metadata.GetSocketMetadataAsync();
/// var inventory = await client.Deposits.GetInventoryAsync();
/// await client.WebSocket.ConnectAsync();
/// </code>
/// </example>
/// </remarks>
public interface ICsGoEmpireClient : IAsyncDisposable
{
    /// <summary>
    /// Provides access to metadata endpoints, including WebSocket authentication credentials.
    /// </summary>
    IMetadataService Metadata { get; }

    /// <summary>
    /// Provides access to user-related endpoints including settings, tipping, and transaction history.
    /// </summary>
    IUserService User { get; }

    /// <summary>
    /// Provides access to inventory and deposit management endpoints.
    /// </summary>
    IDepositService Deposits { get; }

    /// <summary>
    /// Provides access to trade, withdrawal, and marketplace endpoints.
    /// </summary>
    ITradeService Trades { get; }

    /// <summary>
    /// Provides access to auction endpoints for viewing and bidding on items.
    /// </summary>
    IAuctionService Auctions { get; }

    /// <summary>
    /// Provides access to user block list management endpoints.
    /// </summary>
    IBlockListService BlockList { get; }

    /// <summary>
    /// Provides access to trade automation endpoints for managing access tokens and trade checks.
    /// </summary>
    IAutomationService Automation { get; }

    /// <summary>
    /// Provides access to the real-time WebSocket client for marketplace and trade events.
    /// </summary>
    ICsGoEmpireWebSocketClient WebSocket { get; }
}
