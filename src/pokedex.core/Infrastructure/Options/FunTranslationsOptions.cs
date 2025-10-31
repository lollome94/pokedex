namespace Pokedex.Core.Infrastructure.Options;

/// <summary>
/// Configuration options for FunTranslations API integration
/// </summary>
internal sealed class FunTranslationsOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "FunTranslations";

    /// <summary>
    /// Shakespeare translation API endpoint URL
    /// </summary>
    public string ShakespeareApiUrl { get; set; } = "https://api.funtranslations.com/translate/shakespeare.json";

    /// <summary>
    /// Yoda translation API endpoint URL
    /// </summary>
    public string YodaApiUrl { get; set; } = "https://api.funtranslations.com/translate/yoda.json";

    /// <summary>
    /// HTTP request timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
