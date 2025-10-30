using FastEndpoints;

namespace Pokedex.Core.Features.Health;

internal sealed record HealthResponse(string Status, DateTime Timestamp);

internal sealed class HealthEndpoint : EndpointWithoutRequest<HealthResponse>
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Health check endpoint";
            s.Description = "Returns the health status of the API";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        HealthResponse response = new("healthy", DateTime.UtcNow);
        await Send.OkAsync(response, ct);
    }
}