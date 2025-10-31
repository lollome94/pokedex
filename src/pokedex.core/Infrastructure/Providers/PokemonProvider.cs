using PokeApiNet;
using Pokedex.Core.Common.Exceptions;
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
    /// <returns>Pokemon data if found</returns>
    /// <exception cref="ArgumentException">Thrown when pokemonName is null or empty</exception>
    /// <exception cref="PokemonNotFoundException">Thrown when Pokemon is not found</exception>
    /// <exception cref="PokemonDataException">Thrown when data retrieval fails</exception>
    public async Task<Pokemon?> GetPokemonByNameAsync(
        string pokemonName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pokemonName))
        {
            logger.LogWarning("Pokemon name is null or empty");
            throw new ArgumentException("Pokemon name cannot be null or empty", nameof(pokemonName));
        }

        try
        {
            string normalizedName = pokemonName.ToUpperInvariant().Trim();
            logger.LogInformation("Fetching Pokemon data for: {PokemonName}", normalizedName);

            // Fetch Pokemon data from PokeAPI
            Pokemon pokemon = await pokeApiClient.GetResourceAsync<Pokemon>(normalizedName, cancellationToken);

            logger.LogInformation(
                "Successfully retrieved Pokemon: {PokemonName} (ID: {PokemonId})",
                pokemon.Name,
                pokemon.Id);

            return pokemon;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogWarning(
                ex,
                "Pokemon not found: {PokemonName}",
                pokemonName);
            throw new PokemonNotFoundException(pokemonName, ex, setPokemonName: true);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "HTTP error retrieving Pokemon data for: {PokemonName}",
                pokemonName);
            throw new PokemonDataException(
                pokemonName,
                $"HTTP error: {ex.Message}",
                ex);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error retrieving Pokemon data for: {PokemonName}",
                pokemonName);
            throw new PokemonDataException(
                pokemonName,
                "Unexpected error occurred",
                ex);
        }
    }

    /// <summary>
    /// Retrieves Pokemon species information by ID from PokeAPI
    /// Species contains additional data like descriptions, habitat, legendary status
    /// </summary>
    /// <param name="pokemonId">The ID of the Pokemon species to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon species data if found</returns>
    /// <exception cref="ArgumentException">Thrown when pokemonId is invalid</exception>
    /// <exception cref="PokemonNotFoundException">Thrown when Pokemon species is not found</exception>
    /// <exception cref="PokemonDataException">Thrown when data retrieval fails</exception>
    public async Task<PokemonSpecies?> GetPokemonSpeciesByIdAsync(
        int pokemonId,
        CancellationToken cancellationToken = default)
    {
        if (pokemonId <= 0)
        {
            logger.LogWarning("Pokemon ID must be greater than 0, received: {PokemonId}", pokemonId);
            throw new ArgumentException("Pokemon ID must be greater than 0", nameof(pokemonId));
        }

        try
        {
            logger.LogInformation("Fetching Pokemon species data for ID: {PokemonId}", pokemonId);

            // Fetch Pokemon species data from PokeAPI
            PokemonSpecies species = await pokeApiClient.GetResourceAsync<PokemonSpecies>(pokemonId, cancellationToken);

            logger.LogInformation(
                "Successfully retrieved Pokemon species: {SpeciesName} (ID: {SpeciesId})",
                species.Name,
                species.Id);

            return species;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogWarning(
                ex,
                "Pokemon species not found for ID: {PokemonId}",
                pokemonId);
            throw new PokemonNotFoundException(pokemonId.ToString(System.Globalization.CultureInfo.InvariantCulture), ex, setPokemonName: true);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "HTTP error retrieving Pokemon species data for ID: {PokemonId}",
                pokemonId);
            throw new PokemonDataException(
                pokemonId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                $"HTTP error: {ex.Message}",
                ex);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error retrieving Pokemon species data for ID: {PokemonId}",
                pokemonId);
            throw new PokemonDataException(
                pokemonId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                "Unexpected error occurred",
                ex);
        }
    }
}
