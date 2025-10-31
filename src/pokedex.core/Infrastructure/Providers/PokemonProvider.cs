using PokeApiNet;
using Pokedex.Core.Infrastructure.Providers.Interfaces;

namespace Pokedex.Core.Infrastructure.Providers;

/// <summary>
/// Provider implementation for Pokemon API data retrieval
/// Uses PokeApiNet library to fetch Pokemon information
/// </summary>
internal sealed class PokemonProvider(
    PokeApiClient pokeApiClient,
    ILogger<PokemonProvider> logger) : IPokemonProvider
{
    /// <summary>
    /// Retrieves Pokemon information by name from PokeAPI
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to retrieve (case-insensitive)</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon data if found, null if not found or error occurs</returns>
    public async Task<Pokemon?> GetPokemonByNameAsync(
        string pokemonName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                logger.LogWarning("Pokemon name is null or empty");
                return null;
            }

            string normalizedName = pokemonName.ToUpperInvariant().Trim();
            logger.LogInformation("Fetching Pokemon data for: {PokemonName}", normalizedName);

            // Fetch Pokemon data from PokeAPI
            Pokemon pokemon = await pokeApiClient.GetResourceAsync<Pokemon>(normalizedName);

            logger.LogInformation(
                "Successfully retrieved Pokemon: {PokemonName} (ID: {PokemonId})",
                pokemon.Name,
                pokemon.Id);

            return pokemon;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error retrieving Pokemon data for: {PokemonName}",
                pokemonName);
            return null;
        }
    }

    /// <summary>
    /// Retrieves Pokemon species information by ID from PokeAPI
    /// Species contains additional data like descriptions, habitat, legendary status
    /// </summary>
    /// <param name="pokemonId">The ID of the Pokemon species to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon species data if found, null if not found or error occurs</returns>
    public async Task<PokemonSpecies?> GetPokemonSpeciesByIdAsync(
        int pokemonId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (pokemonId <= 0)
            {
                logger.LogWarning("Pokemon ID must be greater than 0, received: {PokemonId}", pokemonId);
                return null;
            }

            logger.LogInformation("Fetching Pokemon species data for ID: {PokemonId}", pokemonId);

            // Fetch Pokemon species data from PokeAPI
            PokemonSpecies species = await pokeApiClient.GetResourceAsync<PokemonSpecies>(pokemonId);

            logger.LogInformation(
                "Successfully retrieved Pokemon species: {SpeciesName} (ID: {SpeciesId})",
                species.Name,
                species.Id);

            return species;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error retrieving Pokemon species data for ID: {PokemonId}",
                pokemonId);
            return null;
        }
    }
}
