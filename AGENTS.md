---
applyTo: '**'
---

# .NET 9 API - GitHub Copilot Instructions

## Project Context
This is a .NET 9.0 ASP.NET Core backend application following Vertical Slice Architecture principles with FastEndpoints for high-performance API endpoints. The project demonstrates modern .NET development practices with clean architecture patterns and efficient API design.

## Vertical Slice Architecture Implementation

### Architecture Structure
- **Features-Based Organization** (`Features/`): Each feature is self-contained with its own endpoint, request, and response classes
- **REPR Pattern**: Request-Endpoint-Response pattern for clear separation of concerns
- **FastEndpoints**: High-performance endpoint framework instead of traditional MVC controllers
- **Minimal Dependencies**: Clean, focused architecture with minimal external dependencies

### Core Architecture Principles
- **Vertical Slice Architecture**: Features are organized vertically, not by technical layers
- **Self-Contained Features**: Each feature folder contains everything needed for that functionality
- **REPR Pattern**: Clear Request → Endpoint → Response flow
- **FastEndpoints**: Modern, high-performance API endpoint framework

## Framework & Technology Stack
- **.NET 9.0** with ASP.NET Core
- **FastEndpoints 7.1.0** for API endpoints
- **Vertical Slice Architecture** for clean feature organization
- **Docker** support with multi-stage builds
- **xUnit** for testing framework

## Coding Standards

### ENGLISH-FIRST REQUIREMENTS
- **ALL labels, messages, comments MUST be in English** - Never use Italian or other languages
- **Variable names**: English descriptive names (e.g., `entityName` not `nomeEntita`)
- **Method names**: English verb-noun patterns (e.g., `GetEntityByName` not `OttieniEntitaPerNome`)
- **Comments**: All documentation, TODO comments, and inline explanations in English
- **Error messages**: User-facing and internal error messages in English
- **Log messages**: All logging messages in English

### C# Language Standards - PRIMARY CONSTRUCTORS MANDATORY
- **ALWAYS use primary constructors** for dependency injection and cleaner code:
  ```csharp
  // ✅ CORRECT - Use primary constructor
  public class GetEntityEndpoint(
      IDataService dataService, 
      ILogger<GetEntityEndpoint> logger)
      : Endpoint<GetEntityRequest, EntityResponse>
  
  // ❌ INCORRECT - Avoid traditional constructor pattern
  public class GetEntityEndpoint : Endpoint<GetEntityRequest, EntityResponse>
  {
      private readonly IDataService _dataService;
      private readonly ILogger<GetEntityEndpoint> _logger;
      
      public GetEntityEndpoint(IDataService dataService, ILogger<GetEntityEndpoint> logger)
      {
          _dataService = dataService;
          _logger = logger;
      }
  }
  ```
- **Use primary constructors for services, repositories, and controllers**
- **Format multi-parameter primary constructors** for readability (3+ parameters):
  ```csharp
  public class ComplexService(
      IFirstDependency firstDependency,
      ISecondDependency secondDependency,
      IThirdDependency thirdDependency,
      ILogger<ComplexService> logger)
  ```
- **Use records for DTOs**: Leverage record types for request/response models:
  ```csharp
  internal sealed record GetEntityRequest(string Name);
  internal sealed record EntityResponse(string Name, string Description, string Category, bool IsActive);
  ```
- **Use async/await** consistently with CancellationToken support
- **Follow SOLID principles** with clear single responsibilities
- **Implement proper cancellation** with CancellationToken parameters

### Naming Conventions
- Use **PascalCase** for classes, methods, properties, and public fields
- Use **camelCase** for local variables and private fields  
- Use **UPPERCASE** for constants
- Use descriptive English names: `GetEntityEndpoint` not `EntityEndpoint`
- Request/Response models: `GetEntityRequest`, `EntityResponse`
- Service classes: `DataService`, `ProcessingService`

### File Organization
- **Features/**: Group by domain (Entities, Health, etc.)
- **Each feature folder**: Endpoint classes + supporting files (requests, responses, services if needed)
- **Flat structure**: Keep it simple - avoid over-engineering with too many subfolders
- **Self-contained**: Each feature should contain everything it needs

### REPR Pattern Implementation

```csharp
// Features/EntityManagement/GetEntity/GetEntity.Contracts.cs
namespace Features.EntityManagement.GetEntity;

public record GetEntityRequest
{
    public string Name { get; init; } = string.Empty;
    public int PageSize { get; init; } = 50;
    public string? ContinuationToken { get; init; }
}

public record GetEntityResponse
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime LastUpdated { get; init; }
}

// Features/EntityManagement/GetEntity/GetEntity.Endpoint.cs
namespace Features.EntityManagement.GetEntity;

public class GetEntityEndpoint(
    IEntityService entityService,
    ILogger<GetEntityEndpoint> logger) 
    : Endpoint<GetEntityRequest, GetEntityResponse>
{
    public override void Configure()
    {
        Get("/api/entities/{name}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get entity information";
            s.Description = "Retrieves entity data with all relevant properties";
            s.Responses[200] = "Entity data retrieved successfully";
            s.Responses[404] = "Entity not found";
        });
    }
    
    public override async Task HandleAsync(
        GetEntityRequest request, 
        CancellationToken cancellationToken)
    {
        var startTime = Stopwatch.StartNew();
        
        // Validate input parameters
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            await Send.ErrorsAsync(cancellation: cancellationToken);
            return;
        }
        
        // Retrieve data from service
        var entity = await entityService.GetByNameAsync(request.Name, cancellationToken);
        
        if (entity == null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }
        
        // Return formatted response
        await Send.OkAsync(new GetEntityResponse
        {
            Name = entity.Name,
            Description = entity.Description,
            Category = entity.Category,
            IsActive = entity.IsActive,
            LastUpdated = entity.LastUpdated
        }, cancellationToken);
        
        logger.LogInformation("Entity {EntityName} retrieved in {ElapsedMs}ms", 
            request.Name, startTime.ElapsedMilliseconds);
    }
}
```

### Project Structure
```
src/
├── pokedex.core/                        # Main application project
│   ├── Common/                          # Utility components shared across all features
│   │   ├── Behavior/                    # Cross-cutting behaviors (if needed)
│   │   └── Middleware/                  # Custom middleware components
│   ├── Features/                        # Vertical slice organization
│   │   └── [FeatureName]/               # Feature-specific folder
│   │       ├── [CommandOrQueryName]/    # Individual command/query implementation
│   │       │   ├── [CommandOrQueryName].Contracts.cs  # Request + Response DTOs
│   │       │   ├── [CommandOrQueryName].Validator.cs  # Request validation (if needed)
│   │       │   ├── [CommandOrQueryName].Endpoint.cs   # Endpoint configuration + handler
│   │       │   └── [CommandOrQueryName].http          # HTTP test file for REST client
│   │       └── Common/                  # Feature-specific shared components
│   │           ├── Models/              # Domain models specific to this feature
│   │           ├── Exceptions/          # Feature-specific exceptions
│   │           └── Constants.cs         # Feature-specific constants
│   ├── Extensions/                      # Application-specific extensions
│   ├── Services/                        # Shared services across all features
│   │   ├── Interfaces/              # Provider interfaces
│   │   │     └── I[Name]Service.cs # Service contracts
│   │   └── [Name]Service.cs        # Concrete service implementations
│   ├── Domain/                          # Domain layer
│   │   ├── Entities/                    # Domain entities
│   │   └── Enums/                       # Domain enumerations (if needed)
│   ├── Infrastructure/                  # Infrastructure layer (Clean Architecture)
│   │   ├── Options/                     # Configuration options classes (if needed)
│   │   ├── Persistence/                 # Entity Framework configuration (if needed)
│   │   ├── Providers/                   # External system integrations (APIs, etc.)
│   │   │   ├── Interfaces/              # Provider interfaces
│   │   │   │   └── I[Name]Provider.cs # Provider contracts
│   │   │   └── [Name]Provider.cs        # Concrete provider implementations
│   │   └── Repositories/                # Data access layer (if needed)
│   │       ├── Interfaces/              # Repository interfaces
│   │       │   └── I[Name]Repository.cs # Repository contracts
│   │       └── [Name]Repository.cs      # Repository implementations
│   ├── Program.cs                       # Application entry point & configuration
│   ├── appsettings.json                # Application configuration
│   └── Properties/
│       └── launchSettings.json          # Development launch profiles
├── tests/
│   └── [project].unit-tests/           # Unit test project
├── Dockerfile                          # Multi-stage Docker build
├── docker-compose.yml                  # Container orchestration
├── Directory.Build.props               # MSBuild global properties
├── Directory.Packages.props            # Centralized package management
└── AGENTS.md                           # GitHub Copilot instructions
```

### Architecture Layer Responsibilities

#### Common Layer
- **Behavior/**: Cross-cutting concerns like validation behaviors, logging behaviors
- **Middleware/**: Custom middleware for request/response processing, error handling

#### Features Layer (Vertical Slice Architecture)
- **[FeatureName]/**: Self-contained feature with all related components
- **[CommandOrQueryName]/**: Individual operation implementation
  - **Contracts.cs**: Request and response DTOs using record types
  - **Validator.cs**: FluentValidation rules for request validation
  - **Endpoint.cs**: FastEndpoints configuration and handler logic
  - **http**: REST client test file for quick endpoint testing
- **Common/**: Feature-specific shared components
  - **Models/**: Domain models specific to this feature only
  - **Exceptions/**: Custom exceptions for this feature
  - **Constants.cs**: Feature-specific constants and configuration

#### Application Layer
- **Extensions/**: Dependency injection setup and application-specific extensions
- **Services/**: Shared business logic services used across multiple features

#### Domain Layer (Clean Architecture)
- **Entities/**: Core business entities with business rules and logic
- **Enums/**: Domain-specific enumerations and value objects

#### Infrastructure Layer (Clean Architecture)
- **Options/**: Strongly-typed configuration classes using Options pattern
- **Persistence/**: Entity Framework DbContext, configurations, and migrations
- **Providers/**: External system integrations (APIs, message queues, etc.)
  - Abstract external dependencies for better testability
- **Repositories/**: Data access implementations with proper abstraction
  - Interface segregation for better maintainability and testing

### Endpoint Structure (FastEndpoints)
```csharp
// ALL comments must be in English
internal sealed class GetEntityEndpoint(ILogger<GetEntityEndpoint> logger) 
    : Endpoint<GetEntityRequest, EntityResponse>
{
    public override void Configure()
    {
        Get("/entities/{name}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get entity information";
            s.Description = "Returns entity data with all relevant properties";
            s.Responses[200] = "Entity data retrieved successfully";
            s.Responses[404] = "Entity not found";
        });
    }
    
    public override async Task HandleAsync(
        GetEntityRequest request, 
        CancellationToken cancellationToken)
    {
        // English comments for business logic
        // Validate input parameters
        // Retrieve data from service/repository
        // Return formatted response
    }
}
```

### Request/Response Models
- Use **record types** for immutable data transfer objects
- Keep models **internal sealed** for encapsulation
- Use descriptive English property names
- Include proper validation attributes when needed

```csharp
internal sealed record GetEntityRequest(string Name);

internal sealed record EntityResponse(
    string Name,
    string Description,
    string Category,
    bool IsActive);

internal sealed record ProcessEntityRequest(string Name, string Action);
```

### Error Handling
- Use appropriate HTTP status codes (200, 404, 400, 500)
- Return consistent error responses
- Log errors appropriately using `ILogger<T>`
- Handle validation errors gracefully

### Mock Data Implementation
- For development/demo purposes, use simple mock data
- Store mock data in static collections or simple data structures
- Include diverse examples with different data types
- Provide both active and inactive/deleted examples

## Code Generation Preferences

### When creating new endpoints:
1. **Use primary constructors** with dependency injection
2. **ALL comments, descriptions, summaries in English**
3. Follow FastEndpoints pattern with comprehensive Configure() method
4. Use record types for request/response models
5. Handle errors gracefully with appropriate HTTP status codes
6. Use CancellationToken parameter with proper cancellation support
7. Keep it simple - avoid over-engineering

### When creating new data services:
1. **Use mock data** for simplicity (no external dependencies initially)
2. **Use English method names** and documentation
3. Implement simple business logic as required
4. Use primary constructor with ILogger dependency
5. Return strongly-typed results with English property names
6. Handle case-insensitive lookups where applicable

### When creating models:
1. Use **record types** for immutable data
2. Keep models **internal sealed** for proper encapsulation
3. Use descriptive English property names
4. Follow consistent naming patterns

### When working with business logic:
1. Implement simple business rules as required
2. Handle edge cases gracefully (return appropriate defaults if processing fails)
3. Use English patterns for any text processing or transformations
4. Keep business logic testable and maintainable

## Testing Considerations
- Use xUnit for unit testing
- Test endpoint behavior with different input scenarios
- Verify business logic works correctly
- Test error scenarios (invalid inputs, etc.)
- Include integration tests for full endpoint workflows

## Performance Guidelines

### Endpoint Optimization
- **FastEndpoints**: Inherently high-performance compared to MVC controllers
- **Async patterns**: Use async/await for all operations (even mock data for consistency)
- **Minimal allocations**: Use efficient string operations and avoid unnecessary object creation
- **Caching**: Consider caching mock data in static collections

### Response Optimization
- **Consistent response structure**: Use same response format across endpoints
- **Minimal data transfer**: Only include necessary fields in responses
- **Proper HTTP status codes**: Use appropriate codes for different scenarios

## Docker & Deployment

### Container Optimization
- **Multi-stage builds**: Separate build and runtime stages
- **Minimal runtime image**: Use aspnet runtime image, not SDK
- **Port configuration**: Default to port 8080 for container runtime
- **Environment variables**: Support configuration through environment variables

### Development Workflow
- **Docker Compose**: Simple single-service setup
- **Hot reload**: Support for development with volume mounts
- **Health checks**: Implement proper container health checking

## Critical Requirements Summary
1. **ENGLISH ONLY**: All code, comments, messages, and documentation must be in English
2. **Vertical Slice Architecture**: Organize by features, not technical layers
3. **FastEndpoints**: Use FastEndpoints framework for all API endpoints
4. **Primary Constructors**: Use modern C# primary constructor syntax when possible
5. **Record Types**: Use records for all request/response models
6. **Mock Data**: Keep it simple with static mock data for development
7. **Async/Await**: All operations should be async with proper cancellation support
8. **Docker Ready**: Ensure application works properly in containerized environment

Always prioritize simplicity, clean code, and English-first standards. The Vertical Slice Architecture ensures each feature is self-contained and easy to understand. Focus on delivering a working API with clean, maintainable code that follows modern .NET practices.

## Common Implementation Patterns

### Standard Endpoint Pattern
```csharp
internal sealed class GetEntityEndpoint(ILogger<GetEntityEndpoint> logger) 
    : Endpoint<GetEntityRequest, EntityResponse>
{
    public override void Configure()
    {
        Get("/entities/{name}");
        AllowAnonymous();
        // Configure with English descriptions
    }
    
    public override async Task HandleAsync(GetEntityRequest request, CancellationToken ct)
    {
        // Lookup entity data from mock collection
        // Handle not found scenarios
        // Return appropriate response
    }
}
```

### Service Pattern
```csharp
internal sealed class ProcessingService(ILogger<ProcessingService> logger)
{
    public string ProcessData(string originalData)
    {
        // Simple data processing logic
        // English error handling and transformation
        // Return processed data or original if processing fails
    }
}
```

### Mock Data Service Pattern
```csharp
internal static class EntityDataService
{
    private static readonly Dictionary<string, EntityData> _entityDatabase = new()
    {
        ["entity1"] = new("entity1", "Description of entity 1...", "category1", true),
        ["entity2"] = new("entity2", "Description of entity 2...", "category2", false),
        // More entity entries
    };
    
    public static EntityData? GetEntity(string name) => 
        _entityDatabase.GetValueOrDefault(name.ToLowerInvariant());
}
```

Remember: Keep implementations simple, focused, and always use English for all naming, comments, and messages.
