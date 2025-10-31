using FastEndpoints;
using Pokedex.Core.Services.Interfaces;

namespace Pokedex.Core.Features.Pokemon.GetPokemon;

/// <summary>
/// Endpoint to retrieve Pokemon information by name
/// </summary>
internal sealed class GetPokemonEndpoint(
    IPokemonService pokemonService,
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

        // Get Pokemon data from business service
        PokemonData? pokemonData = await pokemonService.GetPokemonDataAsync(pokemonName, ct);

        if (pokemonData is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Map to response model
        GetPokemonResponse response = new(
            Name: pokemonData.Name,
            Description: pokemonData.Description,
            Habitat: pokemonData.Habitat,
            IsLegendary: pokemonData.IsLegendary
        );

        logger.LogInformation(
            "Successfully retrieved Pokemon: {PokemonName}",
            pokemonData.Name);

        await Send.OkAsync(response, ct);
    }
}
