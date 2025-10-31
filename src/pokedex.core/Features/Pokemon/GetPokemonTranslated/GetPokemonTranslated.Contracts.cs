namespace Pokedex.Core.Features.Pokemon.GetPokemonTranslated;

/// <summary>
/// Response containing translated Pokemon information
/// </summary>
internal sealed record GetPokemonTranslatedResponse(
    string Name,
    string Description,
    string Habitat,
    bool IsLegendary);
