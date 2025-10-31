using FastEndpoints;
using Pokedex.Core.Infrastructure.Providers.Interfaces;

namespace Pokedex.Core.Features.Pokemon.GetPokemon;

/// <summary>
/// Endpoint to retrieve Pokemon information by name
/// </summary>
internal sealed class GetPokemonEndpoint(
    IPokemonProvider pokemonProvider,
    ILogger<GetPokemonEndpoint> logger)
    : EndpointWithoutRequest<GetPokemonResponse>
{
    public override void Configure()
    {
        Get("/pokemon/{name}");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get Pokemon information";
            s.Description = "Retrieves detailed information about a Pokemon by its name";
            s.Responses[200] = "Pokemon data retrieved successfully";
            s.Responses[404] = "Pokemon not found";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Get name from route parameter
        string pokemonName = Route<string>("name")!;

        // Validate input
        if (string.IsNullOrWhiteSpace(pokemonName))
        {
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        // Fetch Pokemon data from provider
        PokeApiNet.Pokemon? pokemon = await pokemonProvider.GetPokemonByNameAsync(pokemonName, ct);

        if (pokemon is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Fetch Pokemon species to get description and additional details
        PokeApiNet.PokemonSpecies? species = await pokemonProvider.GetPokemonSpeciesByIdAsync(pokemon.Id, ct);

        if (species is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Extract English description from flavor text entries
        string description = species.FlavorTextEntries
            .FirstOrDefault(entry => entry.Language.Name == "en")?
            .FlavorText
            .Replace("\n", " ", StringComparison.Ordinal)
            .Replace("\f", " ", StringComparison.Ordinal)
            .Replace("\r", " ", StringComparison.Ordinal)
            ?? "No description available";

        // Map to response model
        GetPokemonResponse response = new(
            Name: pokemon.Name,
            Description: description,
            Habitat: species.Habitat?.Name ?? "unknown",
            IsLegendary: species.IsLegendary
        );

        logger.LogInformation(
            "Successfully retrieved Pokemon: {PokemonName} with species data",
            pokemon.Name);

        await Send.OkAsync(response, ct);
    }
}
