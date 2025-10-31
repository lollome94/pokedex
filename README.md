# Pokedex API

A production-grade .NET 9 Pokemon information REST API built with **Vertical Slice Architecture** and **FastEndpoints**. This project showcases modern software engineering practices, including clean architecture principles, external API integration, comprehensive error handling, and E2E functional testing with k6.

## What is This?

A high-performance Pokemon API that provides:
- **Pokemon Information** - Name, description, habitat, legendary status fetched from PokeAPI
- **Fun Translations** - Shakespeare and Yoda-style descriptions via FunTranslations API
- **Smart Translation Rules** - Yoda for cave habitat/legendary Pokemon, Shakespeare for others
- **Resilient Error Handling** - Graceful degradation with proper HTTP status codes
- **Health Monitoring** - Service health check endpoint for observability
- **E2E Testing** - Complete k6 test suite for functional validation
- **Docker Ready** - Multi-stage build for production deployment

## Technology Stack

### Core Technologies
- **.NET 9.0** with ASP.NET Core - Latest LTS framework
- **FastEndpoints 7.1.0** - High-performance alternative to MVC controllers (microsecond response times)
- **C# 13** with modern language features (primary constructors, records, pattern matching)
- **Vertical Slice Architecture** - Feature-based organization over technical layers
- **PokeApiNet** - Strongly-typed client for PokeAPI
- **Mapster** - High-performance object mapping

### Infrastructure & DevOps
- **Docker** - Multi-stage builds for optimized production images
- **k6** - Modern load testing tool for E2E functional validation
- **GitHub** - Version control with complete git history

### Key Architectural Decisions
- **REPR Pattern** (Request-Endpoint-Response) - Clear separation of concerns
- **Custom Exception Hierarchy** - Domain-specific error handling
- **Provider Pattern** - Clean abstraction for external API integrations
- **Dependency Injection** - Constructor injection with primary constructors
- **SOLID Principles** - All five principles rigorously applied

## AI Development Support

This project includes **AGENTS.md** - comprehensive instructions for AI-powered development:
- ✅ **GitHub Copilot Ready** - Coding standards and architectural guidance
- ✅ **Architecture Patterns** - Vertical Slice Architecture with REPR pattern
- ✅ **Best Practices** - Primary constructors, async patterns, performance optimization
- ✅ **Code Generation** - Step-by-step feature creation guidelines
- ✅ **Quality Standards** - English-first development with consistent conventions

*Use AGENTS.md with any AI coding assistant to maintain code quality and architectural consistency.*

## Available Endpoints

### `GET /pokemon/{name}`
Get standard Pokemon information with description, habitat, and legendary status.

**Example:**
```bash
GET /pokemon/mewtwo
```
**Response:**
```json
{
  "name": "mewtwo",
  "description": "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
  "habitat": "rare",
  "isLegendary": true
}
```

### `GET /pokemon/translated/{name}`
Get Pokemon info with fun translated description using Shakespeare or Yoda style.

**Translation Rules:**
- **Yoda translation** for Pokemon with cave habitat OR legendary status
- **Shakespeare translation** for all other Pokemon
- Falls back to standard description if translation fails

**Example (Legendary - Yoda):**
```bash
GET /pokemon/translated/mewtwo
```
**Response:**
```json
{
  "name": "mewtwo",
  "description": "Created by a scientist after years of horrific gene splicing and dna engineering experiments, it was.",
  "habitat": "rare",
  "isLegendary": true
}
```

**Example (Standard - Shakespeare):**
```bash
GET /pokemon/translated/pikachu
```
**Response:**
```json
{
  "name": "pikachu",
  "description": "When several of these pokémon gather, their electricity couldst buildeth and cause lightning storms.",
  "habitat": "forest",
  "isLegendary": false
}
```

### `GET /health`
Health check endpoint for monitoring service status.

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-10-31T08:29:57.2705319Z",
  "version": "1.0.0",
  "environment": "Production"
}
```

## Prerequisites

### Local Development
- **.NET 9.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)

### E2E Testing & Docker
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **k6** - Load testing tool for functional tests

#### Install k6

**Windows:**
```powershell
choco install k6        # Chocolatey
scoop install k6        # Scoop
```

**macOS:**
```bash
brew install k6
```

**Linux:**
```bash
sudo gpg -k
sudo gpg --no-default-keyring --keyring /usr/share/keyrings/k6-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb [signed-by=/usr/share/keyrings/k6-archive-keyring.gpg] https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6
```

## How to Run

### Local Development
```bash
dotnet run --project src/pokedex.core
```
→ API available at `http://localhost:5143`

### Docker (Production Mode)
```bash
docker-compose up --build -d    # Build and start
docker-compose logs -f          # View logs
docker-compose down             # Stop
```
→ API available at `http://localhost:5000`

## E2E Testing (TDD Approach)

Tests run against **Release build** in Docker, verifying functionality (not load).

### Run All Tests

**Windows (PowerShell):**
```powershell
.\run-e2e-tests.ps1
```

**macOS/Linux:**
```bash
./run-e2e-tests.sh
```

### Run Specific Tests

**Windows (PowerShell):**
```powershell
.\run-e2e-tests.ps1 -Test health       # Health check only
.\run-e2e-tests.ps1 -Test pokemon      # Pokemon endpoint
.\run-e2e-tests.ps1 -Test translated   # Translation endpoint
```

**macOS/Linux:**
```bash
./run-e2e-tests.sh health
./run-e2e-tests.sh pokemon
./run-e2e-tests.sh translated
```

### What Happens

1. ✅ Checks k6 and Docker installation
2. 🔨 Builds API Docker image (Release mode)
3. 🚀 Starts container on `http://localhost:5000`
4. ⏳ Waits for API readiness
5. 🧪 Runs k6 functional tests
6. 📊 Shows results
7. 💡 Leaves container running for inspection

Stop with: `docker-compose down`

### TDD Workflow

- ✅ **Health** - Implemented (tests pass)
- ✅ **Pokemon** - Implemented (tests pass)
- ✅ **Translation** - Implemented (tests pass)

All core functionality is complete and tested!

## Usage Examples

### curl
```bash
curl http://localhost:5000/health
curl http://localhost:5000/pokemon/mewtwo
curl http://localhost:5000/pokemon/translated/pikachu
```

### PowerShell
```powershell
Invoke-WebRequest -Uri http://localhost:5000/health | Select -ExpandProperty Content
Invoke-WebRequest -Uri http://localhost:5000/pokemon/mewtwo | Select -ExpandProperty Content
```

### http
```bash
http http://localhost:5000/pokemon/mewtwo
```

## Testing with HTTP Files (VS Code)

Each endpoint includes an `.http` file for quick testing directly in VS Code.

### Setup

**1. Install REST Client Extension**

The workspace includes a recommended extension. When you open the project:
- VS Code will prompt: *"Do you want to install the recommended extensions?"*
- Click **Install All** or manually install **REST Client** by Huachao Mao

Alternatively, install manually:
1. Open Extensions (`Ctrl+Shift+X`)
2. Search for **"REST Client"**
3. Install the extension by **Huachao Mao**

**2. Select Environment**

HTTP files use shared variables (like `{{host}}`). To use them:
1. Open any `.http` file (e.g., `Features/Health/GetCurrentHealth/GetCurrentHealth.http`)
2. Look at the **bottom-right corner** of VS Code
3. Click on **"No Environment"** 
4. Select your environment: `Development`, `Production`, or `Docker`

### Available HTTP Files

Each feature includes test files in its folder:
- `Features/Health/GetCurrentHealth/GetCurrentHealth.http`
- `Features/Pokemon/GetPokemon/GetPokemon.http`
- `Features/Pokemon/GetPokemonTranslated/GetPokemonTranslated.http`

### How to Use

1. **Open** the `.http` file
2. **Select environment** (bottom-right corner)
3. **Click** "Send Request" above the HTTP request
4. **View** the response in the right panel

**Example:**
```http
### Get Current Health Status
GET {{host}}/api/health
Accept: application/json
```

### Environment Variables

The `{{host}}` variable automatically resolves based on selected environment:
- **Development**: `http://localhost:5143`
- **Docker**: `http://localhost:5000`
- **Production**: Your production URL

*Tip: You can customize environments by editing `http-client.env.json` in the project root.*

## Development Workflow

### TDD Cycle

**1. RED - Test First**
```bash
.\run-e2e-tests.ps1  # See what fails
```

**2. GREEN - Implement**
```bash
dotnet run --project src/pokedex.core  # Develop with hot reload
# Create endpoint in Features/ following Vertical Slice Architecture
```

**3. GREEN - Verify**
```bash
.\run-e2e-tests.ps1 -Test pokemon  # Tests pass
```

**4. REFACTOR - Improve**
```bash
.\run-e2e-tests.ps1      # Full E2E suite
```

## Production Considerations & Future Enhancements

### What I Would Do Differently for Production

#### ✅ Already Implemented (Production-Ready)
- **Structured Exception Handling** - Custom exceptions with FastEndpoints global handler
- **Proper HTTP Status Codes** - 404 for not found, 429 for rate limits, 500 for server errors
- **Graceful Degradation** - Falls back to standard description if translation fails
- **Docker Multi-Stage Build** - Optimized production image (~100MB)
- **Health Check Endpoint** - For container orchestration and monitoring
- **Comprehensive Logging** - Structured logging with context at every layer
- **SOLID Principles** - Maintainable, testable, and extensible codebase
- **E2E Testing** - k6 functional tests validating all endpoints

#### 🚀 Future Enhancements (Not Yet Implemented)

**1. Caching Strategy**
```csharp
// Redis or in-memory caching for Pokemon data
- Cache PokeAPI responses (TTL: 24h - data rarely changes)
- Cache translations (TTL: 7d - reduces FunTranslations API calls)
- Implement cache invalidation strategy
```

**2. Rate Limiting**
```csharp
// Protect against API abuse
- Implement rate limiting per IP/API key
- Circuit breaker for external APIs (Polly)
- Retry policies with exponential backoff
```

**3. Observability**
```csharp
// Production-grade monitoring
- Prometheus metrics export
- Grafana dashboards
- OpenTelemetry distributed tracing
- Application Insights integration
```

**4. Testing Pyramid**
```csharp
// Current: E2E tests only
// Production needs:
- Unit tests (business logic, providers, services)
- Integration tests (API endpoints with mocked external services)
- Architecture tests (enforce coding standards, dependency rules)
- Performance tests (k6 load tests for scalability validation)
```

**5. API Versioning**
```csharp
// Support multiple API versions
- URL-based versioning (/v1/pokemon, /v2/pokemon)
- Graceful deprecation strategy
```

**6. Authentication & Authorization**
```csharp
// Secure API access
- API key authentication
- JWT bearer tokens
- Rate limit tiers based on subscription level
```

**7. Data Validation**
```csharp
// FluentValidation for complex scenarios
// Current: Simple parameter validation sufficient
// Future: Complex request models with business rules
```

**8. Performance Optimization**
```csharp
// Benchmark project for performance analysis
- BenchmarkDotNet for micro-benchmarks
- Response time optimization
- Memory allocation profiling
```


### Why These Decisions?

**Caching** - PokeAPI data is static; caching reduces latency and external dependencies  
**Rate Limiting** - Protects against abuse and manages FunTranslations API limits  
**Testing Pyramid** - E2E validates behavior; unit tests enable fast iteration  
**Observability** - Critical for debugging distributed systems at scale  
**Circuit Breaker** - Prevents cascade failures when external APIs are down

### Known Limitations

**FunTranslations API Rate Limit**
- Free tier: 10 requests/hour
- Implementation: Throws `TranslationRateLimitException` (HTTP 429)
- Falls back to standard description for graceful degradation
- Production: Would require paid tier or caching strategy

**PokeAPI Availability**
- External dependency without SLA
- Implementation: Proper error handling with `PokemonDataException`
- Production: Would implement caching and circuit breaker

**Testing Coverage**
- E2E tests validate functionality (✅ Complete)
- Unit tests not yet implemented (⚠️ Future work)
- Integration tests not yet implemented (⚠️ Future work)
- Architecture tests not yet implemented (⚠️ Future work)


## 🤖 AI-Powered Test Automation

This project includes **reusable Copilot prompts** for streamlined test management:

### 🧪 Test Runner Prompt
**File**: [`.github/prompt/test-runner.prompt.md`]

Execute k6 E2E tests with automatic platform detection and optional README updates.

**Usage Examples:**
```
@workspace Run E2E tests for pokemon
@workspace Run all E2E tests
@workspace Run E2E tests for health and update README with results
```

**Features:**
- ✅ Automatic platform detection (Windows/macOS/Linux)
- ✅ Prerequisites verification (k6, Docker)
- ✅ Clear test result summaries
- ✅ Optional README status updates
- ✅ Comprehensive error handling

### 🛠️ Test Creator Prompt
**File**: [`.github/test-create.prompt.md`]

Create new k6 E2E tests following established patterns and best practices.

**Usage Examples:**
```
@workspace Create E2E test for Pokemon endpoint testing error handling
@workspace Add test case to pokemon-test for special characters
@workspace Create comprehensive test suite for health endpoint
```

**Features:**
- ✅ Follows project test patterns
- ✅ Includes happy path + error scenarios
- ✅ Creates corresponding .http files
- ✅ Updates documentation automatically
- ✅ Validates against architecture guidelines

**How to Use:**
1. Open GitHub Copilot Chat in VS Code
2. Reference the prompt with `@workspace`
3. Specify your test requirements
4. Copilot handles everything: creation, documentation, validation

*For questions about design decisions, architecture choices, or future enhancements, all details are documented in the codebase with comprehensive comments and dedicated markdown files.*