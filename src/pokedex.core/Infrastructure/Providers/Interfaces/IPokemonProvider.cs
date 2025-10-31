namespace Pokedex.Core.Infrastructure.Providers.Interfaces;

/// <summary>
/// Provider interface for Pokemon data retrieval operations
/// </summary>
internal interface IPokemonProvider
{
    /// <summary>
    /// Retrieves Pokemon information by name
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon data if found, null otherwise</returns>
    Task<PokeApiNet.Pokemon?> GetPokemonByNameAsync(string pokemonName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves Pokemon species information by Pokemon ID
    /// </summary>
    /// <param name="pokemonId">The ID of the Pokemon species to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon species data if found, null otherwise</returns>
    Task<PokeApiNet.PokemonSpecies?> GetPokemonSpeciesByIdAsync(int pokemonId, CancellationToken cancellationToken = default);
}
