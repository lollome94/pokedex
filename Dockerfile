# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy all necessary files for restore
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY src/pokedex.core/*.csproj ./src/pokedex.core/
RUN dotnet restore ./src/pokedex.core/

# Copy source and build
COPY . .
RUN dotnet publish src/pokedex.core/pokedex.core.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "pokedex.core.dll"]