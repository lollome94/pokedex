using System.Text.Json.Serialization;

namespace Pokedex.Core.Extensions;

/// <summary>
/// Estensioni per la configurazione delle opzioni JSON
/// </summary>
internal static class JsonExtensions
{
    /// <summary>
    /// Configura le opzioni globali di System.Text.Json
    /// </summary>
    /// <param name="builder">Il WebApplicationBuilder da configurare</param>
    /// <returns>Il WebApplicationBuilder configurato</returns>
    public static WebApplicationBuilder AddJsonServices(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return builder;
    }
}
