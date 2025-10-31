namespace Pokedex.Core.Common.Exceptions;

/// <summary>
/// Exception thrown when Pokemon data retrieval fails
/// </summary>
public sealed class PokemonDataException : Exception
{
    /// <summary>
    /// The Pokemon name or ID associated with the error
    /// </summary>
    public string? PokemonIdentifier { get; }

    /// <summary>
    /// Creates a new instance of PokemonDataException
    /// </summary>
    public PokemonDataException()
        : base("Failed to retrieve Pokemon data")
    {
    }

    /// <summary>
    /// Creates a new instance of PokemonDataException with message
    /// </summary>
    /// <param name="message">Error message</param>
    public PokemonDataException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of PokemonDataException with message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public PokemonDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates a new instance of PokemonDataException with identifier
    /// </summary>
    /// <param name="pokemonIdentifier">The Pokemon name or ID</param>
    /// <param name="message">Error message</param>
    public PokemonDataException(string pokemonIdentifier, string message)
        : base($"Failed to retrieve Pokemon data for '{pokemonIdentifier}': {message}")
    {
        PokemonIdentifier = pokemonIdentifier;
    }

    /// <summary>
    /// Creates a new instance of PokemonDataException with identifier and inner exception
    /// </summary>
    /// <param name="pokemonIdentifier">The Pokemon name or ID</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public PokemonDataException(string pokemonIdentifier, string message, Exception innerException)
        : base($"Failed to retrieve Pokemon data for '{pokemonIdentifier}': {message}", innerException)
    {
        PokemonIdentifier = pokemonIdentifier;
    }
}
