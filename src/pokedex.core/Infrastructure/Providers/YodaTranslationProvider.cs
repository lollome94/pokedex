using System.Text.Json;
using Pokedex.Core.Infrastructure.Providers.Interfaces;

namespace Pokedex.Core.Infrastructure.Providers;

/// <summary>
/// Provider implementation for Yoda-style text translation using FunTranslations API
/// Single Responsibility: Handles only Yoda translation
/// </summary>
internal sealed class YodaTranslationProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<YodaTranslationProvider> logger) : IYodaTranslationProvider
{

    /// <summary>
    /// Translates text using Yoda translation style
    /// </summary>
    /// <param name="text">The text to translate</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Translated text if successful, null otherwise</returns>
    public async Task<string?> TranslateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                logger.LogWarning("Translation text is null or empty");
                return null;
            }

            logger.LogInformation("Requesting Yoda translation");

            // Create named HttpClient with pre-configured base address
            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientNames.YodaTranslation);

            // Build query string as relative URI
            string queryString = $"?text={Uri.EscapeDataString(text)}";
            Uri requestUri = new(queryString, UriKind.Relative);

            logger.LogDebug("Sending request to Yoda API");

            // Make GET request to FunTranslations API
            HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);

            logger.LogInformation(
                "Yoda API response status: {StatusCode}",
                response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Yoda translation API returned error status code: {StatusCode}",
                    response.StatusCode);
                return null;
            }

            // Parse JSON response
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.LogDebug(
                "Yoda API response length: {ContentLength} bytes",
                responseContent.Length);

            using var document = JsonDocument.Parse(responseContent);

            // Extract translated text from response
            // Response format: { "contents": { "translated": "...", "text": "...", "translation": "yoda" } }
            if (document.RootElement.TryGetProperty("contents", out JsonElement contents) &&
                contents.TryGetProperty("translated", out JsonElement translated))
            {
                string? translatedText = translated.GetString();

                if (!string.IsNullOrWhiteSpace(translatedText))
                {
                    logger.LogInformation("Successfully translated text using Yoda style");
                    return translatedText;
                }
            }

            logger.LogWarning("Could not extract translated text from Yoda API response");
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "HTTP error during Yoda translation: {ErrorMessage}",
                ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            logger.LogError(
                ex,
                "JSON parsing error during Yoda translation: {ErrorMessage}",
                ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error during Yoda translation: {ErrorMessage}",
                ex.Message);
            return null;
        }
    }
}
