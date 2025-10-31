using Pokedex.Core.Infrastructure.Providers.Interfaces;

namespace Pokedex.Core.Infrastructure.Providers;

/// <summary>
/// Provider implementation for Shakespeare-style text translation using FunTranslations API
/// Single Responsibility: Handles only Shakespeare translation
/// </summary>
internal sealed class ShakespeareTranslationProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<ShakespeareTranslationProvider> logger)
    : BaseTranslationProvider(httpClientFactory, logger), IShakespeareTranslationProvider
{
    /// <inheritdoc/>
    protected override string TranslationServiceName => "Shakespeare";

    /// <inheritdoc/>
    protected override string HttpClientName => HttpClientNames.ShakespeareTranslation;

    /// <summary>
    /// Translates text using Shakespeare translation style
    /// </summary>
    /// <param name="text">The text to translate</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Translated text if successful</returns>
    /// <exception cref="Common.Exceptions.TranslationRateLimitException">Thrown when rate limit is exceeded</exception>
    /// <exception cref="Common.Exceptions.TranslationException">Thrown when translation fails</exception>
    public async Task<string?> TranslateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        return await TranslateInternalAsync(text, cancellationToken);
    }
}
