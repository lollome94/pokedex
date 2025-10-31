namespace Pokedex.Core.Common.Exceptions;

/// <summary>
/// Exception thrown when the translation API rate limit is exceeded
/// </summary>
public sealed class TranslationRateLimitException : Exception
{
    /// <summary>
    /// HTTP status code from the API response
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// The translation service that threw the exception (e.g., "Shakespeare", "Yoda")
    /// </summary>
    public string TranslationService { get; }

    /// <summary>
    /// Creates a new instance of TranslationRateLimitException
    /// </summary>
    public TranslationRateLimitException()
        : base("Translation rate limit exceeded")
    {
        TranslationService = string.Empty;
        StatusCode = 429;
    }

    /// <summary>
    /// Creates a new instance of TranslationRateLimitException with message
    /// </summary>
    /// <param name="message">Error message</param>
    public TranslationRateLimitException(string message)
        : base(message)
    {
        TranslationService = string.Empty;
        StatusCode = 429;
    }

    /// <summary>
    /// Creates a new instance of TranslationRateLimitException with message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public TranslationRateLimitException(string message, Exception innerException)
        : base(message, innerException)
    {
        TranslationService = string.Empty;
        StatusCode = 429;
    }

    /// <summary>
    /// Creates a new instance of TranslationRateLimitException
    /// </summary>
    /// <param name="translationService">The translation service name</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Error message from the API</param>
    public TranslationRateLimitException(
        string translationService,
        int statusCode,
        string message)
        : base($"Translation rate limit exceeded for {translationService} service: {message}")
    {
        TranslationService = translationService;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Creates a new instance of TranslationRateLimitException with inner exception
    /// </summary>
    /// <param name="translationService">The translation service name</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Error message from the API</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public TranslationRateLimitException(
        string translationService,
        int statusCode,
        string message,
        Exception innerException)
        : base($"Translation rate limit exceeded for {translationService} service: {message}", innerException)
    {
        TranslationService = translationService;
        StatusCode = statusCode;
    }
}
