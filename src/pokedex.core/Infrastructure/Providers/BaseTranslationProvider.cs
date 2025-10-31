using System.Text.Json;
using Pokedex.Core.Common.Exceptions;

namespace Pokedex.Core.Infrastructure.Providers;

/// <summary>
/// Base implementation for FunTranslations API translation providers
/// Eliminates code duplication between different translation styles
/// </summary>
internal abstract class BaseTranslationProvider(
    IHttpClientFactory httpClientFactory,
    ILogger logger)
{
    /// <summary>
    /// Gets the name of the translation service (e.g., "Shakespeare", "Yoda")
    /// Used for logging and error messages
    /// </summary>
    protected abstract string TranslationServiceName { get; }

    /// <summary>
    /// Gets the HttpClient name configured in IHttpClientFactory
    /// </summary>
    protected abstract string HttpClientName { get; }

    /// <summary>
    /// Translates text using the configured translation style
    /// </summary>
    /// <param name="text">The text to translate</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Translated text if successful</returns>
    /// <exception cref="TranslationRateLimitException">Thrown when rate limit is exceeded (429)</exception>
    /// <exception cref="TranslationException">Thrown when translation fails</exception>
    protected async Task<string> TranslateInternalAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                logger.LogWarning("Translation text is null or empty for {Service}", TranslationServiceName);
                throw new TranslationException(
                    TranslationServiceName,
                    "Translation text cannot be null or empty");
            }

            logger.LogInformation("Requesting {Service} translation", TranslationServiceName);

            // Create named HttpClient with pre-configured base address
            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientName);

            // Build query string as relative URI
            string queryString = $"?text={Uri.EscapeDataString(text)}";
            Uri requestUri = new(queryString, UriKind.Relative);

            logger.LogDebug("Sending request to {Service} API", TranslationServiceName);

            // Make GET request to FunTranslations API
            HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);

            logger.LogInformation(
                "{Service} API response status: {StatusCode}",
                TranslationServiceName,
                response.StatusCode);

            // Parse JSON response content
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.LogDebug(
                "{Service} API response length: {ContentLength} bytes",
                TranslationServiceName,
                responseContent.Length);

            // Check for rate limit error (429)
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                string errorMessage = ExtractErrorMessage(responseContent);
                logger.LogError(
                    "{Service} translation rate limit exceeded: {ErrorMessage}",
                    TranslationServiceName,
                    errorMessage);

                throw new TranslationRateLimitException(
                    TranslationServiceName,
                    (int)response.StatusCode,
                    errorMessage);
            }

            // Check for other error status codes
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = ExtractErrorMessage(responseContent);
                logger.LogWarning(
                    "{Service} translation API returned error status code: {StatusCode} - {ErrorMessage}",
                    TranslationServiceName,
                    response.StatusCode,
                    errorMessage);

                throw new TranslationException(
                    TranslationServiceName,
                    (int)response.StatusCode,
                    errorMessage);
            }

            // Extract translated text from successful response
            string translatedText = ExtractTranslatedText(responseContent);

            logger.LogInformation(
                "Successfully translated text using {Service} style",
                TranslationServiceName);

            return translatedText;
        }
        catch (TranslationRateLimitException)
        {
            // Re-throw custom exceptions as-is
            throw;
        }
        catch (TranslationException)
        {
            // Re-throw custom exceptions as-is
            throw;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "HTTP error during {Service} translation: {ErrorMessage}",
                TranslationServiceName,
                ex.Message);

            throw new TranslationException(
                TranslationServiceName,
                "HTTP request failed",
                ex);
        }
        catch (JsonException ex)
        {
            logger.LogError(
                ex,
                "JSON parsing error during {Service} translation: {ErrorMessage}",
                TranslationServiceName,
                ex.Message);

            throw new TranslationException(
                TranslationServiceName,
                "Failed to parse API response",
                ex);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error during {Service} translation: {ErrorMessage}",
                TranslationServiceName,
                ex.Message);

            throw new TranslationException(
                TranslationServiceName,
                "Unexpected error occurred",
                ex);
        }
    }

    /// <summary>
    /// Extracts the translated text from the API response
    /// Response format: { "contents": { "translated": "...", "text": "...", "translation": "..." } }
    /// </summary>
    /// <param name="responseContent">The JSON response content</param>
    /// <returns>The translated text</returns>
    /// <exception cref="TranslationContentException">Thrown when translated text cannot be extracted (allows fallback)</exception>
    private string ExtractTranslatedText(string responseContent)
    {
        using var document = JsonDocument.Parse(responseContent);

        if (document.RootElement.TryGetProperty("contents", out JsonElement contents) &&
            contents.TryGetProperty("translated", out JsonElement translated))
        {
            string? translatedText = translated.GetString();

            if (!string.IsNullOrWhiteSpace(translatedText))
            {
                return translatedText;
            }
        }

        logger.LogWarning(
            "Could not extract translated text from {Service} API response",
            TranslationServiceName);

        throw new TranslationContentException(
            TranslationServiceName,
            "Could not extract translated text from API response");
    }

    /// <summary>
    /// Extracts error message from the API error response
    /// Response format: { "error": { "code": 429, "message": "..." } }
    /// </summary>
    /// <param name="responseContent">The JSON response content</param>
    /// <returns>The error message or a default message</returns>
    private static string ExtractErrorMessage(string responseContent)
    {
        try
        {
            using var document = JsonDocument.Parse(responseContent);

            if (document.RootElement.TryGetProperty("error", out JsonElement error) &&
                error.TryGetProperty("message", out JsonElement message))
            {
                string? errorMessage = message.GetString();
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return errorMessage;
                }
            }
        }
        catch (JsonException)
        {
            // If we can't parse the error response, return the raw content
            return responseContent;
        }

        return "Unknown error occurred";
    }
}
