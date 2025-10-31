using Mapster;
using Pokedex.Core.Features.Pokemon.GetPokemon;
using Pokedex.Core.Features.Pokemon.GetPokemonTranslated;
using Pokedex.Core.Services.Interfaces;

namespace Pokedex.Core.Infrastructure.Mapping;

/// <summary>
/// Mapster configuration for mapping between domain models and API responses
/// </summary>
internal static class MappingConfiguration
{
    /// <summary>
    /// Registers all mapping configurations for the application
    /// </summary>
    public static void RegisterMappings()
    {
        // Configure mapping from PokemonData to GetPokemonResponse
        TypeAdapterConfig<PokemonData, GetPokemonResponse>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Habitat, src => src.Habitat)
            .Map(dest => dest.IsLegendary, src => src.IsLegendary);

        // Configure mapping from PokemonData to GetPokemonTranslatedResponse
        TypeAdapterConfig<PokemonData, GetPokemonTranslatedResponse>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Habitat, src => src.Habitat)
            .Map(dest => dest.IsLegendary, src => src.IsLegendary);
    }
}
