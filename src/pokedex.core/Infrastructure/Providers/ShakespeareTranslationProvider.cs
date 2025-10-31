using System.Text.Json;
using Pokedex.Core.Infrastructure.Providers.Interfaces;

namespace Pokedex.Core.Infrastructure.Providers;

/// <summary>
/// Provider implementation for Shakespeare-style text translation using FunTranslations API
/// Single Responsibility: Handles only Shakespeare translation
/// </summary>
internal sealed class ShakespeareTranslationProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<ShakespeareTranslationProvider> logger) : IShakespeareTranslationProvider
{

    /// <summary>
    /// Translates text using Shakespeare translation style
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

            logger.LogInformation("Requesting Shakespeare translation");

            // Create named HttpClient with pre-configured base address
            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientNames.ShakespeareTranslation);

            // Build query string as relative URI
            string queryString = $"?text={Uri.EscapeDataString(text)}";
            Uri requestUri = new(queryString, UriKind.Relative);

            logger.LogDebug("Sending request to Shakespeare API");

            // Make GET request to FunTranslations API
            HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);

            logger.LogInformation(
                "Shakespeare API response status: {StatusCode}",
                response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Shakespeare translation API returned error status code: {StatusCode}",
                    response.StatusCode);
                return null;
            }

            // Parse JSON response
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.LogDebug(
                "Shakespeare API response length: {ContentLength} bytes",
                responseContent.Length);

            using var document = JsonDocument.Parse(responseContent);

            // Extract translated text from response
            // Response format: { "contents": { "translated": "...", "text": "...", "translation": "shakespeare" } }
            if (document.RootElement.TryGetProperty("contents", out JsonElement contents) &&
                contents.TryGetProperty("translated", out JsonElement translated))
            {
                string? translatedText = translated.GetString();

                if (!string.IsNullOrWhiteSpace(translatedText))
                {
                    logger.LogInformation("Successfully translated text using Shakespeare style");
                    return translatedText;
                }
            }

            logger.LogWarning("Could not extract translated text from Shakespeare API response");
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "HTTP error during Shakespeare translation: {ErrorMessage}",
                ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            logger.LogError(
                ex,
                "JSON parsing error during Shakespeare translation: {ErrorMessage}",
                ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error during Shakespeare translation: {ErrorMessage}",
                ex.Message);
            return null;
        }
    }
}
