 CsGoEmpire.Api

A professional .NET 10 NuGet package for interacting with the [CSGOEmpire API](https://docs.csgoempire.com/reference/getting-started-with-your-api). Provides strongly-typed clients for all 29 REST endpoints and real-time WebSocket events.

## Features

- **Full REST API coverage** — Metadata, deposits, trades, withdrawals, auctions, block list, automation, tipping, and transaction history.
- **Real-time WebSocket** — Socket.IO v4 client with auto-reconnect and strongly-typed events for marketplace items, trades, and auctions.
- **Built-in rate limiting** — Automatic sliding window tracking (120 req/60s) with retry-on-429 support.
- **Dependency injection** — First-class support for `IServiceCollection` via `AddCsGoEmpireApi()`.
- **Facade pattern** — Single `ICsGoEmpireClient` entry point aggregating all services.
- **Async/await** — Fully asynchronous API with `CancellationToken` support on every method.

## Installation

```bash
dotnet add package CsGoEmpire.Api
```

## Quick Start

### With Dependency Injection (recommended)

```csharp
using CsGoEmpire.Api.Extensions;

// In your Startup / Program.cs
services.AddCsGoEmpireApi(options =>
{
    options.ApiKey = "your-api-key";
});
```

Then inject `ICsGoEmpireClient` wherever needed:

```csharp
using CsGoEmpire.Api;

public class TradingBot
{
    private readonly ICsGoEmpireClient _empire;

    public TradingBot(ICsGoEmpireClient empire)
    {
        _empire = empire;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        // Get user metadata
        var metadata = await _empire.Metadata.GetSocketMetadataAsync(ct);
        Console.WriteLine($"Logged in as: {metadata.User.SteamName}");

        // Browse marketplace
        var items = await _empire.Trades.GetListedItemsAsync(
            new() { PerPage = 50, Page = 1, Sort = "price", Order = "asc" }, ct);

        // Check active trades
        var trades = await _empire.Trades.GetActiveTradesAsync(ct);
    }
}
```

### Without Dependency Injection

```csharp
using CsGoEmpire.Api;
using CsGoEmpire.Api.Extensions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
services.AddCsGoEmpireApi(options =>
{
    options.ApiKey = "your-api-key";
});

await using var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<ICsGoEmpireClient>();

var metadata = await client.Metadata.GetSocketMetadataAsync();
```

## Available Services

| Service | Property | Description |
|---|---|---|
| `IMetadataService` | `client.Metadata` | WebSocket auth credentials and user profile |
| `IUserService` | `client.User` | Settings, tipping, transaction history |
| `IDepositService` | `client.Deposits` | Inventory, create/cancel deposits, pricing |
| `ITradeService` | `client.Trades` | Active trades, withdrawals, marketplace items |
| `IAuctionService` | `client.Auctions` | View and bid on auctions |
| `IBlockListService` | `client.BlockList` | Block/unblock users |
| `IAutomationService` | `client.Automation` | Access tokens and trade checks |
| `ICsGoEmpireWebSocketClient` | `client.WebSocket` | Real-time marketplace and trade events |

## Deposits & Inventory

```csharp
// Get CS2 inventory
var inventory = await client.Deposits.GetInventoryAsync(cancellationToken: ct);

// Create a deposit (list items for sale)
var deposit = await client.Deposits.CreateDepositAsync(new CreateDepositRequest
{
    Items = new List<DepositItem>
    {
        new() { Id = 123, CoinValue = 5000 }
    }
}, ct);

// Check deposit status
var status = await client.Deposits.CheckDepositStatusAsync(deposit.TrackingCode, ct);

// Update listing price
await client.Deposits.UpdateListingPriceAsync(depositId, new UpdateListingPriceRequest
{
    CoinValue = 6000
}, ct);

// Cancel a deposit
await client.Deposits.CancelDepositAsync(depositId, ct);
```

## Trades & Withdrawals

```csharp
// Browse marketplace items
var items = await client.Trades.GetListedItemsAsync(new GetListedItemsRequest
{
    PerPage = 50,
    Page = 1,
    PriceMin = 1000,
    PriceMax = 50000,
    Sort = "price",
    Order = "asc"
}, ct);

// Withdraw (purchase) an item
var trade = await client.Trades.CreateWithdrawalAsync(depositId, cancellationToken: ct);

// Get active trades
var activeTrades = await client.Trades.GetActiveTradesAsync(ct);

// Mark trade as sent / received
await client.Trades.MarkAsSentAsync(depositId, ct);
await client.Trades.MarkAsReceivedAsync(tradeofferId, ct);
```

## Auctions

```csharp
// Get active auctions
var auctions = await client.Auctions.GetActiveAuctionsAsync(ct);

// Place a bid
await client.Auctions.PlaceBidAsync(depositId, new PlaceBidRequest
{
    BidValue = 10000
}, ct);
```

## WebSocket (Real-time Events)

```csharp
// Subscribe to events
client.WebSocket.OnNewItem += (sender, items) =>
{
    foreach (var item in items)
        Console.WriteLine($"New item: {item.MarketName} — {item.MarketValue} coins");
};

client.WebSocket.OnTradeStatus += (sender, trade) =>
{
    Console.WriteLine($"Trade {trade.Data.Id} status: {trade.Data.StatusMessage}");
};

client.WebSocket.OnAuctionUpdate += (sender, updates) =>
{
    foreach (var update in updates)
        Console.WriteLine($"Auction {update.Id}: highest bid {update.AuctionHighestBid}");
};

client.WebSocket.OnConnected += (sender, _) =>
    Console.WriteLine("WebSocket connected!");

client.WebSocket.OnDisconnected += (sender, _) =>
    Console.WriteLine("WebSocket disconnected.");

client.WebSocket.OnError += (sender, ex) =>
    Console.WriteLine($"WebSocket error: {ex.Message}");

// Connect (auto-authenticates)
await client.WebSocket.ConnectAsync(ct);

// Later: disconnect
await client.WebSocket.DisconnectAsync(ct);
```

## Automation

```csharp
// Check automation status
var status = await client.Automation.GetStatusAsync(ct);

// Update Steam access token
await client.Automation.UpdateAccessTokenAsync(new UpdateAccessTokenRequest
{
    AccessToken = "steam-access-token"
}, ct);

// Trigger trade check
await client.Automation.CheckTradesAsync(ct);

// Delete access token
await client.Automation.DeleteAccessTokenAsync(ct);
```

## Block List

```csharp
// Block a user
await client.BlockList.BlockUserAsync("76561198012345678", ct);

// Get blocked users
var blocked = await client.BlockList.GetBlockedUsersAsync(ct);

// Unblock a user
await client.BlockList.UnblockUserAsync("76561198012345678", ct);
```

## User & Tipping

```csharp
// Update settings
await client.User.UpdateSettingsAsync(new UpdateSettingsRequest
{
    TradeUrl = "https://steamcommunity.com/tradeoffer/new/?partner=..."
}, ct);

// Send a tip
await client.User.SendTipAsync(new TipRequest
{
    SteamId = "76561198012345678",
    Amount = "1000"
}, ct);

// Get transaction history
var history = await client.User.GetTransactionHistoryAsync(page: 1, ct);
```

## Configuration

| Option | Default | Description |
|---|---|---|
| `ApiKey` | *(required)* | Your CSGOEmpire API key |
| `BaseUrl` | `https://csgoempire.com/api/v2` | REST API base URL |
| `WebSocketUrl` | `wss://trade.csgoempire.com/s/?EIO=3&transport=websocket` | WebSocket endpoint |
| `MaxRequestsPerMinute` | `120` | Rate limit threshold |

## Error Handling

The library throws typed exceptions for API errors:

- **`CsGoEmpireApiException`** — General API errors (4xx/5xx) with `StatusCode`, `Message`, and `ErrorKey`.
- **`RateLimitExceededException`** — Thrown when the rate limit is exceeded (HTTP 429). The built-in `RateLimitHandler` automatically retries after the cooldown period.

```csharp
try
{
    await client.Trades.CreateWithdrawalAsync(depositId, cancellationToken: ct);
}
catch (RateLimitExceededException)
{
    Console.WriteLine("Rate limited — the handler will retry automatically.");
}
catch (CsGoEmpireApiException ex)
{
    Console.WriteLine($"API error [{ex.StatusCode}]: {ex.Message} (key: {ex.ErrorKey})");
}
```

## Pricing

All monetary values in the API use **coincents** (1 coin = 100 coincents).

## License

MIT