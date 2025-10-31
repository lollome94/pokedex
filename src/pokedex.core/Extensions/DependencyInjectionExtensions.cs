using PokeApiNet;
using Pokedex.Core.Infrastructure.Providers;
using Pokedex.Core.Infrastructure.Providers.Interfaces;

namespace Pokedex.Core.Extensions;

internal static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers infrastructure services
    /// </summary>
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        // Register PokeApiClient as a singleton (recommended by library documentation)
        builder.Services.AddSingleton<PokeApiClient>();

        // Register Pokemon provider
        builder.Services.AddScoped<IPokemonProvider, PokemonProvider>();

        return builder;
    }

    /// <summary>
    /// Registers all application services
    /// </summary>
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        return builder;
    }
}
