# Pokedex API

The goal of this small application is to create a Pokedex! It returns Pokemon information through REST APIs.

This application has two endpoints:

## Endpoints

### GET /pokemon/{pokemon name}

This endpoint responds with pokemon name, standard description, habitat, and is_legendary status.

**Example Response:**
```json
{
  "name": "mewtwo",
  "description": "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
  "habitat": "rare",
  "isLegendary": true
}
```

### GET /pokemon/translated/{pokemon name}

This endpoint responds with pokemon name, translated description (Yoda-style), habitat, and is_legendary status.

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

Health check endpoint that returns API status.

**Example Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-10-30T21:01:49.1113109Z"
}
```

## Technology Stack

- **.NET 9** - Latest .NET framework
- **FastEndpoints** - High-performance endpoint framework
- **Vertical Slice Architecture** - Clean architecture pattern
- **REPR Pattern** - Request-Endpoint-Response pattern

## Docker Setup

### Prerequisites

- Docker Desktop installed and running

### Quick Start

**Build and run:**
```bash
docker-compose up --build -d
```

**Stop:**
```bash
docker-compose down
```

The API will be available at: **http://localhost:5000**

### Docker Files Explained

- **`Dockerfile`** - Multi-stage build (SDK for build, runtime for production)
- **`docker-compose.yml`** - Simple service definition with port mapping
- **`.dockerignore`** - Excludes unnecessary files from build context

## Usage Examples

### Test the health endpoint
```bash
curl http://localhost:5000/health
```

### Get a Pokemon
```bash
curl http://localhost:5000/pokemon/mewtwo
curl http://localhost:5000/pokemon/pikachu
```

### Get a Pokemon with translated description
```bash
curl http://localhost:5000/pokemon/translated/mewtwo
curl http://localhost:5000/pokemon/translated/pikachu
```

## Available Pokemon (Mock Data)

The API currently returns mock data for these Pokemon:
- **mewtwo** - Legendary psychic Pokemon
- **pikachu** - Electric mouse Pokemon  
- **charizard** - Fire/flying dragon Pokemon
- **Any other name** - Returns generic Pokemon data

## Project Structure

```
src/
  pokedex.core/
    Features/
      Health/           # Health check endpoint
      Pokemon/          # Pokemon-related endpoints
        GetPokemon/     # Standard Pokemon endpoint
        GetTranslatedPokemon/  # Translated Pokemon endpoint
    Program.cs          # Application entry point
```

This project follows **Vertical Slice Architecture** where each feature is self-contained with its own request, response, and endpoint classes.