using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using PokeApiNet;
using Pokedex.Core.Infrastructure;
using Pokedex.Core.Infrastructure.Mapping;
using Pokedex.Core.Infrastructure.Options;
using Pokedex.Core.Infrastructure.Providers;
using Pokedex.Core.Infrastructure.Providers.Interfaces;
using Pokedex.Core.Services;
using Pokedex.Core.Services.Interfaces;

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

        // Register FunTranslations options
        builder.Services.Configure<FunTranslationsOptions>(
            builder.Configuration.GetSection(FunTranslationsOptions.SectionName));

        // Register named HttpClients for FunTranslations API with configuration
        builder.Services.AddHttpClient(HttpClientNames.ShakespeareTranslation, (serviceProvider, client) =>
        {
            FunTranslationsOptions options = serviceProvider
                .GetRequiredService<IOptions<FunTranslationsOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.ShakespeareApiUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient(HttpClientNames.YodaTranslation, (serviceProvider, client) =>
        {
            FunTranslationsOptions options = serviceProvider
                .GetRequiredService<IOptions<FunTranslationsOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.YodaApiUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Register Pokemon provider
        builder.Services.AddScoped<IPokemonProvider, PokemonProvider>();

        // Register Translation providers with Single Responsibility
        builder.Services.AddScoped<IShakespeareTranslationProvider, ShakespeareTranslationProvider>();
        builder.Services.AddScoped<IYodaTranslationProvider, YodaTranslationProvider>();

        return builder;
    }

    /// <summary>
    /// Registers all application services
    /// </summary>
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        // Register Mapster for object mapping
        TypeAdapterConfig config = new();
        MappingConfiguration.RegisterMappings();
        config.Scan(typeof(Program).Assembly);
        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IMapper, ServiceMapper>();

        // Register business services
        builder.Services.AddScoped<IPokemonService, PokemonService>();

        return builder;
    }
}
