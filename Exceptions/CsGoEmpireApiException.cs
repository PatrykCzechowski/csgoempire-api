using System.Net;

namespace CsGoEmpire.Api.Exceptions;

/// <summary>
/// Represents an error returned by the CSGOEmpire API.
/// </summary>
public class CsGoEmpireApiException : Exception
{
    /// <summary>
    /// The HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// An optional error key returned by the API for programmatic error handling.
    /// </summary>
    public string? ErrorKey { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CsGoEmpireApiException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="errorKey">An optional error key from the API response.</param>
    /// <param name="innerException">An optional inner exception.</param>
    public CsGoEmpireApiException(
        string message,
        HttpStatusCode statusCode,
        string? errorKey = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorKey = errorKey;
    }
}
