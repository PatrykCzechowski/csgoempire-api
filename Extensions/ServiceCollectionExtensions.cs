using CsGoEmpire.Api.Configuration;
using CsGoEmpire.Api.Http;
using CsGoEmpire.Api.Services;
using CsGoEmpire.Api.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CsGoEmpire.Api.Extensions;

/// <summary>
/// Extension methods for registering CSGOEmpire API services with the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the CSGOEmpire API client and all related services with the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method registers the following components:
    /// <list type="bullet">
    ///   <item><see cref="ICsGoEmpireClient"/> — the main facade aggregating all services.</item>
    ///   <item><see cref="IMetadataService"/> — WebSocket authentication and user metadata.</item>
    ///   <item><see cref="IUserService"/> — user settings, tipping, and transaction history.</item>
    ///   <item><see cref="IDepositService"/> — inventory and deposit management.</item>
    ///   <item><see cref="ITradeService"/> — trades, withdrawals, and marketplace browsing.</item>
    ///   <item><see cref="IAuctionService"/> — auction viewing and bidding.</item>
    ///   <item><see cref="IBlockListService"/> — user block list management.</item>
    ///   <item><see cref="IAutomationService"/> — trade automation and access tokens.</item>
    ///   <item><see cref="ICsGoEmpireWebSocketClient"/> — real-time WebSocket events.</item>
    /// </list>
    /// </para>
    /// <para>
    /// REST services are registered as scoped. The WebSocket client is registered as a singleton
    /// to maintain a persistent connection across the application lifetime.
    /// The <see cref="ICsGoEmpireClient"/> facade is registered as scoped.
    /// </para>
    /// </remarks>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">A delegate to configure the <see cref="CsGoEmpireOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configure"/> is <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// services.AddCsGoEmpireApi(options =>
    /// {
    ///     options.ApiKey = "your-api-key";
    ///     options.MaxRequestsPerMinute = 120;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddCsGoEmpireApi(
        this IServiceCollection services,
        Action<CsGoEmpireOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        // Configuration
        services.Configure(configure);

        // HTTP infrastructure
        services.AddTransient<RateLimitHandler>();

        services.AddHttpClient("CsGoEmpire", (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<CsGoEmpireOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<RateLimitHandler>();

        services.AddScoped(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient("CsGoEmpire");
            var logger = sp.GetRequiredService<ILogger<CsGoEmpireHttpClient>>();
            return new CsGoEmpireHttpClient(httpClient, logger);
        });

        // REST services
        services.AddScoped<IMetadataService, MetadataService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDepositService, DepositService>();
        services.AddScoped<ITradeService, TradeService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IBlockListService, BlockListService>();
        services.AddScoped<IAutomationService, AutomationService>();

        // WebSocket client (singleton for persistent connection)
        services.AddSingleton<ICsGoEmpireWebSocketClient, CsGoEmpireWebSocketClient>();

        // Main facade client
        services.AddScoped<ICsGoEmpireClient, CsGoEmpireClient>();

        return services;
    }
}
