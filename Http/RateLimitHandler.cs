using System.Collections.Concurrent;
using System.Net;
using CsGoEmpire.Api.Configuration;
using CsGoEmpire.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CsGoEmpire.Api.Http;

/// <summary>
/// A <see cref="DelegatingHandler"/> that enforces the CSGOEmpire API rate limit
/// by tracking requests within a sliding window and throttling when necessary.
/// </summary>
/// <remarks>
/// Uses a sliding window algorithm to track the number of requests made within the last 60 seconds.
/// When the limit is approached, the handler delays outgoing requests to stay within bounds.
/// If HTTP 429 is received from the server, it waits and retries automatically.
/// </remarks>
internal sealed class RateLimitHandler : DelegatingHandler
{
    private readonly ILogger<RateLimitHandler> _logger;
    private readonly int _maxRequestsPerMinute;
    private readonly ConcurrentQueue<DateTimeOffset> _requestTimestamps = new();
    private readonly SemaphoreSlim _throttleSemaphore = new(1, 1);

    private const int WindowSeconds = 60;
    private const int RetryAfterDefaultSeconds = 60;
    private const int MaxRetries = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitHandler"/> class.
    /// </summary>
    /// <param name="options">The CSGOEmpire API options containing rate limit configuration.</param>
    /// <param name="logger">The logger instance.</param>
    public RateLimitHandler(IOptions<CsGoEmpireOptions> options, ILogger<RateLimitHandler> logger)
    {
        _logger = logger;
        _maxRequestsPerMinute = options.Value.MaxRequestsPerMinute;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var retryCount = 0;

        while (true)
        {
            await WaitForAvailableSlotAsync(cancellationToken).ConfigureAwait(false);

            _requestTimestamps.Enqueue(DateTimeOffset.UtcNow);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.TooManyRequests)
                return response;

            // HTTP 429 — rate limit exceeded on server side
            retryCount++;
            if (retryCount > MaxRetries)
            {
                _logger.LogError("Rate limit exceeded after {MaxRetries} retries for {Url}",
                    MaxRetries, request.RequestUri);
                throw new RateLimitExceededException(
                    retryAfterSeconds: RetryAfterDefaultSeconds);
            }

            var retryAfter = GetRetryAfterSeconds(response);
            _logger.LogWarning(
                "Rate limited (HTTP 429) on {Url}. Retry {RetryCount}/{MaxRetries} after {RetryAfter}s",
                request.RequestUri, retryCount, MaxRetries, retryAfter);

            response.Dispose();

            await Task.Delay(TimeSpan.FromSeconds(retryAfter), cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Waits until a request slot is available within the sliding window.
    /// </summary>
    private async Task WaitForAvailableSlotAsync(CancellationToken cancellationToken)
    {
        await _throttleSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            PurgeExpiredTimestamps();

            if (_requestTimestamps.Count < _maxRequestsPerMinute)
                return;

            // Calculate how long to wait for the oldest request to expire from the window
            if (_requestTimestamps.TryPeek(out var oldest))
            {
                var waitUntil = oldest.AddSeconds(WindowSeconds);
                var delay = waitUntil - DateTimeOffset.UtcNow;

                if (delay > TimeSpan.Zero)
                {
                    _logger.LogDebug(
                        "Rate limit threshold reached ({Count}/{Max}). Delaying {DelayMs}ms",
                        _requestTimestamps.Count, _maxRequestsPerMinute, (int)delay.TotalMilliseconds);

                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    PurgeExpiredTimestamps();
                }
            }
        }
        finally
        {
            _throttleSemaphore.Release();
        }
    }

    /// <summary>
    /// Removes timestamps older than the sliding window from the queue.
    /// </summary>
    private void PurgeExpiredTimestamps()
    {
        var cutoff = DateTimeOffset.UtcNow.AddSeconds(-WindowSeconds);
        while (_requestTimestamps.TryPeek(out var ts) && ts < cutoff)
        {
            _requestTimestamps.TryDequeue(out _);
        }
    }

    /// <summary>
    /// Extracts the Retry-After value from the response, falling back to a default.
    /// </summary>
    private static int GetRetryAfterSeconds(HttpResponseMessage response)
    {
        if (response.Headers.RetryAfter?.Delta is { } delta)
            return (int)Math.Ceiling(delta.TotalSeconds);

        if (response.Headers.RetryAfter?.Date is { } date)
        {
            var diff = date - DateTimeOffset.UtcNow;
            return diff > TimeSpan.Zero ? (int)Math.Ceiling(diff.TotalSeconds) : RetryAfterDefaultSeconds;
        }

        return RetryAfterDefaultSeconds;
    }
}
