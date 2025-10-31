using FastEndpoints;
using Pokedex.Core.Infrastructure.Providers.Interfaces;
using Pokedex.Core.Services.Interfaces;

namespace Pokedex.Core.Features.Pokemon.GetPokemonTranslated;

/// <summary>
/// Endpoint to retrieve Pokemon information with translated description
/// Applies Yoda translation for cave habitat or legendary Pokemon, Shakespeare translation otherwise
/// Uses dedicated translation providers following Single Responsibility Principle
/// </summary>
internal sealed class GetPokemonTranslatedEndpoint(
    IPokemonService pokemonService,
    IShakespeareTranslationProvider shakespeareTranslationProvider,
    IYodaTranslationProvider yodaTranslationProvider,
    ILogger<GetPokemonTranslatedEndpoint> logger)
    : EndpointWithoutRequest<GetPokemonTranslatedResponse>
{
    public override void Configure()
    {
        Get("/pokemon/translated/{name}");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get Pokemon information with translated description";
            s.Description = "Retrieves Pokemon information with description translated to Yoda (cave/legendary) or Shakespeare style";
            s.Responses[200] = "Pokemon data with translated description retrieved successfully";
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

        // Apply translation based on Pokemon characteristics
        string translatedDescription = await GetTranslatedDescriptionAsync(
            pokemonData.Description,
            pokemonData.Habitat,
            pokemonData.IsLegendary,
            ct);

        // Map to response model
        GetPokemonTranslatedResponse response = new(
            Name: pokemonData.Name,
            Description: translatedDescription,
            Habitat: pokemonData.Habitat,
            IsLegendary: pokemonData.IsLegendary
        );

        logger.LogInformation(
            "Successfully retrieved translated Pokemon: {PokemonName}",
            pokemonData.Name);

        await Send.OkAsync(response, ct);
    }

    /// <summary>
    /// Applies appropriate translation based on Pokemon characteristics
    /// Rules: Use Yoda for cave habitat or legendary Pokemon, Shakespeare otherwise
    /// Falls back to original description if translation fails
    /// Uses dedicated providers following Single Responsibility Principle
    /// </summary>
    private async Task<string> GetTranslatedDescriptionAsync(
        string originalDescription,
        string? habitat,
        bool isLegendary,
        CancellationToken ct)
    {
        // Determine which provider to use based on business rules
        bool shouldUseYoda = habitat?.Equals("cave", StringComparison.OrdinalIgnoreCase) == true || isLegendary;

        logger.LogInformation(
            "Applying {TranslationType} translation for Pokemon with habitat: {Habitat}, legendary: {IsLegendary}",
            shouldUseYoda ? "Yoda" : "Shakespeare",
            habitat ?? "unknown",
            isLegendary);

        // Use the appropriate dedicated translation provider
        string? translatedText = shouldUseYoda
            ? await yodaTranslationProvider.TranslateAsync(originalDescription, ct)
            : await shakespeareTranslationProvider.TranslateAsync(originalDescription, ct);

        // Fallback to original description if translation fails
        if (string.IsNullOrWhiteSpace(translatedText))
        {
            logger.LogInformation(
                "Translation failed or returned empty, using original description");
            return originalDescription;
        }

        logger.LogInformation(
            "Successfully applied {TranslationType} translation",
            shouldUseYoda ? "Yoda" : "Shakespeare");

        return translatedText;
    }
}
