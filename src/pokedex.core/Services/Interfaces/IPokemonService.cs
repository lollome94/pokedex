namespace Pokedex.Core.Services.Interfaces;

/// <summary>
/// Business service for Pokemon data processing and orchestration
/// </summary>
internal interface IPokemonService
{
    /// <summary>
    /// Retrieves Pokemon information with formatted description
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon data with formatted description, or null if not found</returns>
    Task<PokemonData?> GetPokemonDataAsync(string pokemonName, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents Pokemon data with formatted description and metadata
/// </summary>
internal sealed record PokemonData(
    string Name,
    string Description,
    string Habitat,
    bool IsLegendary);
