namespace Pokedex.Core.Common.Exceptions;

/// <summary>
/// Exception thrown when a Pokemon cannot be found
/// </summary>
public sealed class PokemonNotFoundException : Exception
{
    /// <summary>
    /// The name of the Pokemon that was not found
    /// </summary>
    public string PokemonName { get; }

    /// <summary>
    /// Creates a new instance of PokemonNotFoundException
    /// </summary>
    public PokemonNotFoundException()
        : base("Pokemon not found")
    {
        PokemonName = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of PokemonNotFoundException with message
    /// </summary>
    /// <param name="message">Error message</param>
    public PokemonNotFoundException(string message)
        : base(message)
    {
        PokemonName = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of PokemonNotFoundException with message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public PokemonNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
        PokemonName = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of PokemonNotFoundException with Pokemon name
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon that was not found</param>
    /// <param name="setPokemonName">Set to true to use pokemonName as PokemonName property</param>
    internal PokemonNotFoundException(string pokemonName, bool setPokemonName)
        : base($"Pokemon '{pokemonName}' not found")
    {
        PokemonName = setPokemonName ? pokemonName : string.Empty;
    }

    /// <summary>
    /// Creates a new instance of PokemonNotFoundException with Pokemon name and inner exception
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon that was not found</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    /// <param name="setPokemonName">Set to true to use pokemonName as PokemonName property</param>
    internal PokemonNotFoundException(string pokemonName, Exception innerException, bool setPokemonName)
        : base($"Pokemon '{pokemonName}' not found", innerException)
    {
        PokemonName = setPokemonName ? pokemonName : string.Empty;
    }
}
