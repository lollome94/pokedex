namespace Pokedex.Core.Infrastructure;

/// <summary>
/// HTTP client names for dependency injection and IHttpClientFactory
/// </summary>
internal static class HttpClientNames
{
    /// <summary>
    /// Named HttpClient for Shakespeare translation API
    /// </summary>
    public const string ShakespeareTranslation = "ShakespeareTranslation";

    /// <summary>
    /// Named HttpClient for Yoda translation API
    /// </summary>
    public const string YodaTranslation = "YodaTranslation";
}
