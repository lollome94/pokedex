# Pokedex API

A modern .NET 9 Pokemon information API built with Vertical Slice Architecture and FastEndpoints. Provides Pokemon data including descriptions, habitats, legendary status, and Yoda-style translations.

## API Endpoints

### GET /pokemon/{name}
Retrieves standard Pokemon information including name, description, habitat, and legendary status.

**Example Request:**
```bash
GET /pokemon/mewtwo
```

**Example Response:**
```json
{
  "name": "mewtwo",
  "description": "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
  "habitat": "rare",
  "isLegendary": true
}
```

### GET /pokemon/translated/{name}
Returns Pokemon information with Yoda-style translated description for a unique twist on the standard data.

**Example Request:**
```bash
GET /pokemon/translated/mewtwo
```

**Example Response:**
```json
{
  "name": "mewtwo",
  "description": "Created by a scientist after years of horrific gene splicing and dna engineering experiments, it was.",
  "habitat": "rare",
  "isLegendary": true
}
```

### GET /health
Health check endpoint that returns API operational status.

**Example Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-10-30T21:01:49.1113109Z"
}
```

## Technology Stack

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core** - Cross-platform web framework
- **FastEndpoints 7.1.0** - High-performance API endpoints
- **Vertical Slice Architecture** - Feature-based organization
- **Docker** - Containerization support

## AI Development Support

This project includes comprehensive **AGENTS.md** instructions specifically designed for AI-powered development:

- **✅ GitHub Copilot Ready** - Complete coding standards and architectural guidance
- **✅ Clean Architecture Patterns** - Vertical Slice Architecture with REPR pattern implementation
- **✅ Best Practices Enforcement** - Primary constructors, async patterns, and performance optimization
- **✅ Code Generation Guidelines** - Step-by-step instructions for creating new features
- **✅ Quality Standards** - English-first development with consistent naming conventions

*The AGENTS.md file can be used with any AI coding assistant to maintain code quality and architectural consistency.*

## Quick Start

### Prerequisites
- **.NET 9.0 SDK**
- **Docker Desktop** (optional)

### Local Development
```bash
# Clone and setup
git clone <repository-url>
cd pokedex

# Run the application
dotnet run --project src/pokedex.core
```
API available at: **https://localhost:5143**

### Docker
```bash
# Build and run
docker-compose up --build -d

# Stop
docker-compose down
```
API available at: **http://localhost:5000**

## Usage Examples

**Health Check:**
```bash
curl http://localhost:5000/health
```

**Get Pokemon:**
```bash
curl http://localhost:5000/pokemon/pikachu
curl http://localhost:5000/pokemon/mewtwo
```

**Get Translated Pokemon:**
```bash
curl http://localhost:5000/pokemon/translated/pikachu
```

## Available Pokemon

The API includes mock data for popular Pokemon:

**Legendary Pokemon:**
- **Mewtwo** - Psychic legendary with rare habitat
- **Articuno** - Ice/Flying legendary found in icy caves
- **Zapdos** - Electric/Flying legendary from power plants

**Standard Pokemon:**
- **Pikachu** - Electric mouse Pokemon from forests
- **Charizard** - Fire/Flying dragon from mountain regions  
- **Gyarados** - Water/Flying serpent from water areas
- **Alakazam** - Psychic humanoid from urban environments

*Note: Any Pokemon name not in the database returns a generic response.*