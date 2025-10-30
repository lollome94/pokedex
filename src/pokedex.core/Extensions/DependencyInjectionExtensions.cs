namespace Pokedex.Core.Extensions;

internal static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers infrastructure services
    /// </summary>
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {

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
