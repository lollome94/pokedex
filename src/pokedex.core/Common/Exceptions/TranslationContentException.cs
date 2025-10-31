namespace Pokedex.Core.Common.Exceptions;

/// <summary>
/// Exception thrown when translation API responds successfully but the translated content cannot be extracted
/// This is a non-critical error that allows fallback to original description
/// </summary>
public sealed class TranslationContentException : TranslationException
{
    /// <summary>
    /// Creates a new instance of TranslationContentException
    /// </summary>
    public TranslationContentException()
        : base("Translation content could not be extracted")
    {
    }

    /// <summary>
    /// Creates a new instance of TranslationContentException with message
    /// </summary>
    /// <param name="message">Error message</param>
    public TranslationContentException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of TranslationContentException with message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public TranslationContentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates a new instance of TranslationContentException
    /// </summary>
    /// <param name="translationService">The translation service name</param>
    /// <param name="message">Error message</param>
    public TranslationContentException(string translationService, string message)
        : base(translationService, message)
    {
    }
}
