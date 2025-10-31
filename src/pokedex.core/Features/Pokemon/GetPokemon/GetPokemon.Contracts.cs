namespace Pokedex.Core.Features.Pokemon.GetPokemon;

/// <summary>
/// Response containing Pokemon information
/// </summary>
internal sealed record GetPokemonResponse(
    string Name,
    string Description,
    string Habitat,
    bool IsLegendary);
