using System.Net;

namespace CsGoEmpire.Api.Exceptions;

/// <summary>
/// Thrown when the CSGOEmpire API rate limit (HTTP 429) is exceeded.
/// </summary>
public sealed class RateLimitExceededException : CsGoEmpireApiException
{
    /// <summary>
    /// The number of seconds to wait before retrying, if provided by the API.
    /// </summary>
    public int? RetryAfterSeconds { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitExceededException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="retryAfterSeconds">Optional number of seconds to wait before retrying.</param>
    /// <param name="innerException">An optional inner exception.</param>
    public RateLimitExceededException(
        string message = "API rate limit exceeded. Please wait before making additional requests.",
        int? retryAfterSeconds = null,
        Exception? innerException = null)
        : base(message, HttpStatusCode.TooManyRequests, "rate_limit_exceeded", innerException)
    {
        RetryAfterSeconds = retryAfterSeconds;
    }
}
