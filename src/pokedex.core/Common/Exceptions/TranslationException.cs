namespace Pokedex.Core.Common.Exceptions;

/// <summary>
/// Exception thrown when a translation operation fails
/// </summary>
public class TranslationException : Exception
{
    /// <summary>
    /// The translation service that threw the exception (e.g., "Shakespeare", "Yoda")
    /// </summary>
    public string TranslationService { get; }

    /// <summary>
    /// HTTP status code from the API response, if available
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Creates a new instance of TranslationException
    /// </summary>
    public TranslationException()
        : base("Translation failed")
    {
        TranslationService = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of TranslationException with message
    /// </summary>
    /// <param name="message">Error message</param>
    public TranslationException(string message)
        : base(message)
    {
        TranslationService = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of TranslationException with message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public TranslationException(string message, Exception innerException)
        : base(message, innerException)
    {
        TranslationService = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of TranslationException
    /// </summary>
    /// <param name="translationService">The translation service name</param>
    /// <param name="message">Error message</param>
    public TranslationException(string translationService, string message)
        : base($"Translation failed for {translationService} service: {message}")
    {
        TranslationService = translationService;
    }

    /// <summary>
    /// Creates a new instance of TranslationException with status code
    /// </summary>
    /// <param name="translationService">The translation service name</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Error message</param>
    public TranslationException(string translationService, int statusCode, string message)
        : base($"Translation failed for {translationService} service (Status {statusCode}): {message}")
    {
        TranslationService = translationService;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Creates a new instance of TranslationException with inner exception
    /// </summary>
    /// <param name="translationService">The translation service name</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public TranslationException(string translationService, string message, Exception innerException)
        : base($"Translation failed for {translationService} service: {message}", innerException)
    {
        TranslationService = translationService;
    }
}
