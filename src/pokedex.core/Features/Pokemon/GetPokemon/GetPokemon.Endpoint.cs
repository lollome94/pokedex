using FastEndpoints;
using Pokedex.Core.Services.Interfaces;

namespace Pokedex.Core.Features.Pokemon.GetPokemon;

/// <summary>
/// Endpoint to retrieve Pokemon information by name
/// Uses Mapster for automatic object mapping
/// </summary>
internal sealed class GetPokemonEndpoint(
    IPokemonService pokemonService,
    MapsterMapper.IMapper mapper,
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

        // Map to response model using Mapster
        GetPokemonResponse response = mapper.Map<GetPokemonResponse>(pokemonData);

        logger.LogInformation(
            "Successfully retrieved Pokemon: {PokemonName}",
            pokemonData.Name);

        await Send.OkAsync(response, ct);
    }
}
