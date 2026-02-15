using CsGoEmpire.Api.Services;
using CsGoEmpire.Api.WebSocket;

namespace CsGoEmpire.Api;

/// <summary>
/// Default implementation of <see cref="ICsGoEmpireClient"/> that acts as a facade,
/// aggregating all CSGOEmpire API services into a single entry point.
/// </summary>
/// <remarks>
/// <para>
/// This class is designed to be resolved through dependency injection. All service dependencies
/// are injected via the constructor, and the client delegates to the appropriate service for each operation.
/// </para>
/// <para>
/// Disposing this client will dispose the <see cref="WebSocket"/> client, closing any active
/// WebSocket connections. REST service instances are not disposable and are managed by the DI container.
/// </para>
/// </remarks>
public sealed class CsGoEmpireClient : ICsGoEmpireClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CsGoEmpireClient"/> class.
    /// </summary>
    /// <param name="metadata">The metadata service instance.</param>
    /// <param name="user">The user service instance.</param>
    /// <param name="deposits">The deposit service instance.</param>
    /// <param name="trades">The trade service instance.</param>
    /// <param name="auctions">The auction service instance.</param>
    /// <param name="blockList">The block list service instance.</param>
    /// <param name="automation">The automation service instance.</param>
    /// <param name="webSocket">The WebSocket client instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters is <c>null</c>.</exception>
    public CsGoEmpireClient(
        IMetadataService metadata,
        IUserService user,
        IDepositService deposits,
        ITradeService trades,
        IAuctionService auctions,
        IBlockListService blockList,
        IAutomationService automation,
        ICsGoEmpireWebSocketClient webSocket)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(deposits);
        ArgumentNullException.ThrowIfNull(trades);
        ArgumentNullException.ThrowIfNull(auctions);
        ArgumentNullException.ThrowIfNull(blockList);
        ArgumentNullException.ThrowIfNull(automation);
        ArgumentNullException.ThrowIfNull(webSocket);

        Metadata = metadata;
        User = user;
        Deposits = deposits;
        Trades = trades;
        Auctions = auctions;
        BlockList = blockList;
        Automation = automation;
        WebSocket = webSocket;
    }

    /// <inheritdoc />
    public IMetadataService Metadata { get; }

    /// <inheritdoc />
    public IUserService User { get; }

    /// <inheritdoc />
    public IDepositService Deposits { get; }

    /// <inheritdoc />
    public ITradeService Trades { get; }

    /// <inheritdoc />
    public IAuctionService Auctions { get; }

    /// <inheritdoc />
    public IBlockListService BlockList { get; }

    /// <inheritdoc />
    public IAutomationService Automation { get; }

    /// <inheritdoc />
    public ICsGoEmpireWebSocketClient WebSocket { get; }

    /// <summary>
    /// Disposes the client asynchronously, closing the WebSocket connection if active.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await WebSocket.DisposeAsync().ConfigureAwait(false);
    }
}
