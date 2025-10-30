using System.Diagnostics;
using FastEndpoints;
using Pokedex.Core.Features.Health.GetCurrentHealth;

namespace Pokedex.Core.Features.Health.GetCurrentHealth;

internal sealed class GetCurrentHealthEndpoint(
    ILogger<GetCurrentHealthEndpoint> logger,
    IWebHostEnvironment environment) 
    : EndpointWithoutRequest<GetCurrentHealthResponse>
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get current health status";
            s.Description = "Retrieves the current health status of the API with detailed information";
            s.Responses[200] = "Health status retrieved successfully";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var startTime = Stopwatch.StartNew();
        
        // Create detailed health response
        var response = new GetCurrentHealthResponse(
            Status: "healthy",
            Timestamp: DateTime.UtcNow,
            Version: "1.0.0",
            Environment: environment.EnvironmentName);
        
        await Send.OkAsync(response, ct);
        
        logger.LogInformation(
            "Health check retrieved in {ElapsedMs}ms for environment {Environment}", 
            startTime.ElapsedMilliseconds,
            environment.EnvironmentName);
    }
}
