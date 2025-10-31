# Pokedex API

A modern .NET 9 Pokemon information API built with **Vertical Slice Architecture** and **FastEndpoints**. This project demonstrates high-performance API development with clean architecture principles, mock data services, and comprehensive TDD testing approach.

## What is This?

A production-ready Pokemon API that provides:
- **Pokemon Information** - Name, description, habitat, legendary status
- **Yoda Translation** - Fun translated descriptions in Yoda-speak style
- **Health Monitoring** - Service health check endpoint
- **Mock Data** - Pre-configured Pokemon database for testing
- **E2E Testing** - Complete k6 test suite for TDD workflow

## Technology Stack

- **.NET 9.0** with ASP.NET Core
- **FastEndpoints 7.1.0** - High-performance endpoint framework
- **Vertical Slice Architecture** - Feature-based organization
- **Docker** - Multi-stage builds for containerization
- **k6** - Load testing tool for E2E functional tests

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
Get Pokemon info with Yoda-style translated description.

**Example:**
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

### `GET /health`
Health check endpoint for monitoring service status.

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-10-30T21:01:49.1113109Z"
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
- ❌ **Pokemon** - Not yet implemented (tests fail - RED phase)
- ❌ **Translation** - Not yet implemented (tests fail - RED phase)

Implement endpoints to turn tests green!

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

### httpie
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
- `Features/Pokemon/GetPokemon/GetPokemon.http` *(when implemented)*
- `Features/Pokemon/GetTranslatedPokemon/GetTranslatedPokemon.http` *(when implemented)*

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

## Mock Data

The API includes pre-configured Pokemon data:

**Legendary:** Mewtwo, Articuno, Zapdos  
**Standard:** Pikachu, Charizard, Gyarados, Alakazam

*Unknown Pokemon names will return appropriate error responses.*

## Troubleshooting

**k6 not found:** Install k6 (see Prerequisites)  
**Docker not running:** Start Docker Desktop  
**API failed to start:** `docker-compose logs` then `docker-compose down -v`  
**PowerShell policy error:** `Set-ExecutionPolicy RemoteSigned -Scope CurrentUser`  
**Port in use:** Kill process on port 5143 (local) or 5000 (Docker)  
**.NET 9 not found:** Install .NET 9 SDK

## Architecture

This project follows:
- **Vertical Slice Architecture** - Feature-based organization
- **REPR Pattern** - Request → Endpoint → Response
- **FastEndpoints** - High-performance endpoints
- **TDD Approach** - Test-first development
- **English-first** - All code and documentation in English

See **[AGENTS.md](AGENTS.md)** for complete coding standards, architecture guidelines, and project structure.