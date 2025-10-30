namespace Pokedex.Core.Features.Health.GetCurrentHealth;

internal sealed record GetCurrentHealthResponse(
    string Status,
    DateTime Timestamp,
    string Version,
    string Environment);
