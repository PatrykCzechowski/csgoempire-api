using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsGoEmpire.Api.Exceptions;
using Microsoft.Extensions.Logging;

namespace CsGoEmpire.Api.Http;

/// <summary>
/// Internal HTTP client wrapper for communicating with the CSGOEmpire API.
/// Handles serialization, deserialization, and error mapping.
/// </summary>
internal sealed class CsGoEmpireHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CsGoEmpireHttpClient> _logger;

    /// <summary>
    /// Shared JSON serializer options configured for the CSGOEmpire API (snake_case, enum as string, ignore nulls).
    /// </summary>
    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="CsGoEmpireHttpClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client provided by <see cref="IHttpClientFactory"/>.</param>
    /// <param name="logger">The logger instance.</param>
    public CsGoEmpireHttpClient(HttpClient httpClient, ILogger<CsGoEmpireHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Sends a GET request and deserializes the response body.
    /// </summary>
    /// <typeparam name="T">The expected response type.</typeparam>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The deserialized response.</returns>
    public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("GET {Uri}", requestUri);

        using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        return await HandleResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a POST request with a JSON body and deserializes the response body.
    /// </summary>
    /// <typeparam name="T">The expected response type.</typeparam>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="body">The request body to serialize as JSON. Pass <c>null</c> for an empty body.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The deserialized response.</returns>
    public async Task<T> PostAsync<T>(string requestUri, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("POST {Uri}", requestUri);

        using var response = await SendJsonAsync(HttpMethod.Post, requestUri, body, cancellationToken).ConfigureAwait(false);
        return await HandleResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a POST request with a JSON body without expecting a typed response body.
    /// </summary>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="body">The request body to serialize as JSON. Pass <c>null</c> for an empty body.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public async Task PostAsync(string requestUri, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("POST {Uri}", requestUri);

        using var response = await SendJsonAsync(HttpMethod.Post, requestUri, body, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PATCH request with a JSON body and deserializes the response body.
    /// </summary>
    /// <typeparam name="T">The expected response type.</typeparam>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="body">The request body to serialize as JSON.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The deserialized response.</returns>
    public async Task<T> PatchAsync<T>(string requestUri, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("PATCH {Uri}", requestUri);

        using var response = await SendJsonAsync(HttpMethod.Patch, requestUri, body, cancellationToken).ConfigureAwait(false);
        return await HandleResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PATCH request with a JSON body without expecting a typed response body.
    /// </summary>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="body">The request body to serialize as JSON.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public async Task PatchAsync(string requestUri, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("PATCH {Uri}", requestUri);

        using var response = await SendJsonAsync(HttpMethod.Patch, requestUri, body, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PUT request with a JSON body and deserializes the response body.
    /// </summary>
    /// <typeparam name="T">The expected response type.</typeparam>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="body">The request body to serialize as JSON.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The deserialized response.</returns>
    public async Task<T> PutAsync<T>(string requestUri, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("PUT {Uri}", requestUri);

        using var response = await SendJsonAsync(HttpMethod.Put, requestUri, body, cancellationToken).ConfigureAwait(false);
        return await HandleResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PUT request with a JSON body without expecting a typed response body.
    /// </summary>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="body">The request body to serialize as JSON.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public async Task PutAsync(string requestUri, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("PUT {Uri}", requestUri);

        using var response = await SendJsonAsync(HttpMethod.Put, requestUri, body, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a DELETE request and deserializes the response body.
    /// </summary>
    /// <typeparam name="T">The expected response type.</typeparam>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The deserialized response.</returns>
    public async Task<T> DeleteAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("DELETE {Uri}", requestUri);

        using var response = await _httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
        return await HandleResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a DELETE request without expecting a typed response body.
    /// </summary>
    /// <param name="requestUri">The relative request URI.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public async Task DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("DELETE {Uri}", requestUri);

        using var response = await _httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends an HTTP request with a JSON body.
    /// </summary>
    private async Task<HttpResponseMessage> SendJsonAsync(
        HttpMethod method,
        string requestUri,
        object? body,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, requestUri);

        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        return await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deserializes a successful response or throws the appropriate exception.
    /// </summary>
    private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);

        var result = await response.Content
            .ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            throw new CsGoEmpireApiException(
                "API returned a null or empty response body.",
                response.StatusCode);
        }

        return result;
    }

    /// <summary>
    /// Validates the HTTP response status and throws mapped exceptions for errors.
    /// </summary>
    private async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var responseBody = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogError(
            "API error {StatusCode} for {Url}: {Body}",
            (int)response.StatusCode, response.RequestMessage?.RequestUri, responseBody);

        // Try to extract structured error information from the response body
        string? message = null;
        string? errorKey = null;
        try
        {
            var errorDoc = JsonDocument.Parse(responseBody);
            if (errorDoc.RootElement.TryGetProperty("message", out var msgProp))
                message = msgProp.GetString();
            if (errorDoc.RootElement.TryGetProperty("error_key", out var keyProp))
                errorKey = keyProp.GetString();
        }
        catch (JsonException)
        {
            // Response body is not valid JSON — use the raw body as the message
        }

        message ??= $"API request failed with status code {(int)response.StatusCode}.";

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var retryAfter = response.Headers.RetryAfter?.Delta is { } delta
                ? (int)Math.Ceiling(delta.TotalSeconds)
                : (int?)null;

            throw new RateLimitExceededException(message, retryAfter);
        }

        throw new CsGoEmpireApiException(message, response.StatusCode, errorKey);
    }
}
