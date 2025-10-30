# .NET 9 API - GitHub Copilot Instructions

## Project Context
This is a .NET 9.0 ASP.NET Core backend application following Vertical Slice Architecture principles with FastEndpoints for high-performance API endpoints. The project demonstrates modern .NET development practices with clean architecture patterns and efficient API design.

## Framework & Technology Stack
- **.NET 9.0** with ASP.NET Core
- **FastEndpoints 7.1.0** for API endpoints
- **Vertical Slice Architecture** for clean feature organization
- **Docker** support with multi-stage builds
- **xUnit** for testing framework

## Architecture Principles

### Vertical Slice Architecture
- **Features-Based Organization** (`Features/`): Each feature is self-contained with its own endpoint, request, and response classes
- **REPR Pattern**: Request-Endpoint-Response pattern for clear separation of concerns
- **Self-Contained Features**: Each feature folder contains everything needed for that functionality
- **FastEndpoints**: High-performance endpoint framework instead of traditional MVC controllers
- **Minimal Dependencies**: Clean, focused architecture with minimal external dependencies

## Coding Standards

### ENGLISH-FIRST REQUIREMENTS ⚠️
- **ALL labels, messages, comments MUST be in English** - Never use Italian or other languages
- **Variable names**: English descriptive names (e.g., `entityName` not `nomeEntita`)
- **Method names**: English verb-noun patterns (e.g., `GetEntityByName` not `OttieniEntitaPerNome`)
- **Comments**: All documentation, TODO comments, and inline explanations in English
- **Error messages**: User-facing and internal error messages in English
- **Log messages**: All logging messages in English

### C# Language Standards

#### PRIMARY CONSTRUCTORS - MANDATORY ✅
**ALWAYS use primary constructors** for dependency injection and cleaner code:

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

**Format multi-parameter primary constructors** for readability (3+ parameters):
```csharp
public class ComplexService(
    IFirstDependency firstDependency,
    ISecondDependency secondDependency,
    IThirdDependency thirdDependency,
    ILogger<ComplexService> logger)
```

#### Record Types for DTOs
```csharp
internal sealed record GetEntityRequest(string Name);
internal sealed record EntityResponse(string Name, string Description, string Category, bool IsActive);
```

#### Other C# Standards
- **Use async/await** consistently with CancellationToken support
- **Follow SOLID principles** with clear single responsibilities
- **Implement proper cancellation** with CancellationToken parameters

### Naming Conventions
- **PascalCase**: classes, methods, properties, public fields
- **camelCase**: local variables, private fields
- **UPPERCASE**: constants
- **Descriptive English names**: `GetEntityEndpoint` not `EntityEndpoint`
- **Request/Response models**: `GetEntityRequest`, `EntityResponse`
- **Service classes**: `DataService`, `ProcessingService`

## Project Structure

```
src/
├── pokedex.core/                        # Main application project
│   ├── Common/                          # Shared utility components
│   │   ├── Behavior/                    # Cross-cutting behaviors
│   │   └── Middleware/                  # Custom middleware
│   ├── Features/                        # Vertical slice organization
│   │   └── [FeatureName]/               # Feature-specific folder
│   │       ├── [CommandOrQueryName]/    # Individual operation
│   │       │   ├── [CommandOrQueryName].Contracts.cs  # Request + Response DTOs
│   │       │   ├── [CommandOrQueryName].Validator.cs  # Request validation
│   │       │   ├── [CommandOrQueryName].Endpoint.cs   # Endpoint + handler
│   │       │   └── [CommandOrQueryName].http          # HTTP test file
│   │       └── Common/                  # Feature-specific shared components
│   │           ├── Models/              # Domain models for this feature
│   │           ├── Exceptions/          # Feature-specific exceptions
│   │           └── Constants.cs         # Feature constants
│   ├── Extensions/                      # Application-specific extensions
│   ├── Services/                        # Shared services across features
│   │   ├── Interfaces/                  # Service contracts
│   │   │   └── I[Name]Service.cs
│   │   └── [Name]Service.cs             # Service implementations
│   ├── Domain/                          # Domain layer
│   │   ├── Entities/                    # Domain entities
│   │   └── Enums/                       # Domain enumerations
│   ├── Infrastructure/                  # Infrastructure layer
│   │   ├── Options/                     # Configuration options
│   │   ├── Persistence/                 # Entity Framework configuration
│   │   ├── Providers/                   # External system integrations
│   │   │   ├── Interfaces/              # Provider contracts
│   │   │   │   └── I[Name]Provider.cs
│   │   │   └── [Name]Provider.cs        # Provider implementations
│   │   └── Repositories/                # Data access layer
│   │       ├── Interfaces/              # Repository contracts
│   │       │   └── I[Name]Repository.cs
│   │       └── [Name]Repository.cs      # Repository implementations
│   ├── Program.cs                       # Application entry point
│   ├── appsettings.json                 # Application configuration
│   └── Properties/
│       └── launchSettings.json          # Development launch profiles
├── tests/
│   └── [project].unit-tests/           # Unit test project
├── Dockerfile                           # Multi-stage Docker build
├── docker-compose.yml                   # Container orchestration
├── Directory.Build.props                # MSBuild global properties
├── Directory.Packages.props             # Centralized package management
└── AGENTS.md                            # GitHub Copilot instructions
```

### Architecture Layer Responsibilities

#### Common Layer
- **Behavior/**: Cross-cutting concerns (validation, logging behaviors)
- **Middleware/**: Request/response processing, error handling

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
  - **Constants.cs**: Feature-specific constants

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
- **Repositories/**: Data access implementations with proper abstraction

## Implementation Patterns

### FastEndpoints Pattern

```csharp
// Features/EntityManagement/GetEntity/GetEntity.Contracts.cs
namespace Features.EntityManagement.GetEntity;

internal sealed record GetEntityRequest(string Name);

internal sealed record GetEntityResponse(
    string Name,
    string Description,
    string Category,
    bool IsActive,
    DateTime LastUpdated);

// Features/EntityManagement/GetEntity/GetEntity.Endpoint.cs
namespace Features.EntityManagement.GetEntity;

internal sealed class GetEntityEndpoint(
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
        CancellationToken ct)
    {
        var startTime = Stopwatch.StartNew();
        
        // Validate input parameters
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }
        
        // Retrieve data from service
        var entity = await entityService.GetByNameAsync(request.Name, ct);
        
        if (entity == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        // Return formatted response
        await Send.OkAsync(new GetEntityResponse(
            entity.Name,
            entity.Description,
            entity.Category,
            entity.IsActive,
            entity.LastUpdated
        ), ct);
        
        logger.LogInformation("Entity {EntityName} retrieved in {ElapsedMs}ms", 
            request.Name, startTime.ElapsedMilliseconds);
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
        return originalData;
    }
}
```

### Mock Data Service Pattern

```csharp
internal static class EntityDataService
{
    private static readonly Dictionary<string, EntityData> _entityDatabase = new()
    {
        ["entity1"] = new("entity1", "Description of entity 1", "category1", true),
        ["entity2"] = new("entity2", "Description of entity 2", "category2", false)
    };
    
    public static EntityData? GetEntity(string name) => 
        _entityDatabase.GetValueOrDefault(name.ToLowerInvariant());
}
```

## Code Generation Guidelines

### Creating New Endpoints
1. Use primary constructors with dependency injection
2. ALL comments, descriptions, summaries in English
3. Follow FastEndpoints pattern with comprehensive Configure() method
4. Use record types for request/response models
5. Handle errors gracefully with appropriate HTTP status codes
6. Use CancellationToken parameter with proper cancellation support
7. Keep it simple - avoid over-engineering

### Creating New Services
1. Use mock data for simplicity (no external dependencies initially)
2. Use English method names and documentation
3. Implement simple business logic as required
4. Use primary constructor with ILogger dependency
5. Return strongly-typed results with English property names
6. Handle case-insensitive lookups where applicable

### Creating Models
1. Use record types for immutable data
2. Keep models internal sealed for proper encapsulation
3. Use descriptive English property names
4. Follow consistent naming patterns

## Error Handling & Validation
- Use appropriate HTTP status codes (200, 404, 400, 500)
- Return consistent error responses
- Log errors appropriately using `ILogger<T>`
- Handle validation errors gracefully

## Mock Data Implementation
- For development/demo purposes, use simple mock data
- Store mock data in static collections or simple data structures
- Include diverse examples with different data types
- Provide both active and inactive/deleted examples

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

## Testing Considerations
- Use xUnit for unit testing
- Test endpoint behavior with different input scenarios
- Verify business logic works correctly
- Test error scenarios (invalid inputs, etc.)
- Include integration tests for full endpoint workflows

## Critical Requirements Summary ⚠️
1. **ENGLISH ONLY**: All code, comments, messages, and documentation must be in English
2. **Vertical Slice Architecture**: Organize by features, not technical layers
3. **FastEndpoints**: Use FastEndpoints framework for all API endpoints
4. **Primary Constructors**: Use modern C# primary constructor syntax
5. **Record Types**: Use records for all request/response models
6. **Mock Data**: Keep it simple with static mock data for development
7. **Async/Await**: All operations should be async with proper cancellation support
8. **Docker Ready**: Ensure application works properly in containerized environment

Always prioritize simplicity, clean code, and English-first standards. The Vertical Slice Architecture ensures each feature is self-contained and easy to understand. Focus on delivering a working API with clean, maintainable code that follows modern .NET practices.
