namespace Pokedex.Core.Infrastructure.Providers.Interfaces;

/// <summary>
/// Provider interface for Shakespeare-style text translation using FunTranslations API
/// </summary>
internal interface IShakespeareTranslationProvider
{
    /// <summary>
    /// Translates text using Shakespeare translation style
    /// </summary>
    /// <param name="text">The text to translate</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Translated text if successful, null otherwise</returns>
    Task<string?> TranslateAsync(string text, CancellationToken cancellationToken = default);
}
