using Pokedex.Core.Infrastructure.Providers.Interfaces;
using Pokedex.Core.Services.Interfaces;

namespace Pokedex.Core.Services;

/// <summary>
/// Business service implementation for Pokemon data processing
/// Handles data retrieval, formatting, and business logic orchestration
/// </summary>
internal sealed class PokemonService(
    IPokemonProvider pokemonProvider,
    ILogger<PokemonService> logger) : IPokemonService
{
    private const string EnglishLanguageCode = "en";
    private const string DefaultDescription = "No description available";
    private const string DefaultHabitat = "unknown";

    /// <summary>
    /// Retrieves Pokemon information with formatted description
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Pokemon data with formatted description, or null if not found</returns>
    public async Task<PokemonData?> GetPokemonDataAsync(
        string pokemonName,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving Pokemon data for: {PokemonName}", pokemonName);

        // Fetch Pokemon data from provider
        PokeApiNet.Pokemon? pokemon = await pokemonProvider.GetPokemonByNameAsync(pokemonName, cancellationToken);

        if (pokemon is null)
        {
            logger.LogWarning("Pokemon not found: {PokemonName}", pokemonName);
            return null;
        }

        // Fetch Pokemon species to get description and additional details
        PokeApiNet.PokemonSpecies? species = await pokemonProvider.GetPokemonSpeciesByIdAsync(pokemon.Id, cancellationToken);

        if (species is null)
        {
            logger.LogWarning("Pokemon species not found for Pokemon ID: {PokemonId}", pokemon.Id);
            return null;
        }

        // Extract and format English description
        string description = ExtractEnglishDescription(species);

        logger.LogInformation(
            "Successfully retrieved Pokemon data: {PokemonName} (Habitat: {Habitat}, Legendary: {IsLegendary})",
            pokemon.Name,
            species.Habitat?.Name ?? DefaultHabitat,
            species.IsLegendary);

        return new PokemonData(
            Name: pokemon.Name,
            Description: description,
            Habitat: species.Habitat?.Name ?? DefaultHabitat,
            IsLegendary: species.IsLegendary);
    }

    /// <summary>
    /// Extracts English description from Pokemon species flavor text entries
    /// Formats the description by removing special characters and line breaks
    /// </summary>
    private string ExtractEnglishDescription(PokeApiNet.PokemonSpecies species)
    {
        string? rawDescription = species.FlavorTextEntries
            .FirstOrDefault(entry => entry.Language.Name == EnglishLanguageCode)?
            .FlavorText;

        if (string.IsNullOrWhiteSpace(rawDescription))
        {
            logger.LogDebug("No English description found for species: {SpeciesName}", species.Name);
            return DefaultDescription;
        }

        // Clean up description by replacing special characters and line breaks with spaces
        string cleanDescription = rawDescription
            .Replace("\n", " ", StringComparison.Ordinal)
            .Replace("\f", " ", StringComparison.Ordinal)
            .Replace("\r", " ", StringComparison.Ordinal);

        logger.LogDebug(
            "Extracted and cleaned description for species: {SpeciesName}, length: {DescriptionLength}",
            species.Name,
            cleanDescription.Length);

        return cleanDescription;
    }
}
