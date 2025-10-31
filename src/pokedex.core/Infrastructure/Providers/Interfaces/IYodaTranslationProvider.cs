namespace Pokedex.Core.Infrastructure.Providers.Interfaces;

/// <summary>
/// Provider interface for Yoda-style text translation using FunTranslations API
/// </summary>
internal interface IYodaTranslationProvider
{
    /// <summary>
    /// Translates text using Yoda translation style
    /// </summary>
    /// <param name="text">The text to translate</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Translated text if successful, null otherwise</returns>
    Task<string?> TranslateAsync(string text, CancellationToken cancellationToken = default);
}
