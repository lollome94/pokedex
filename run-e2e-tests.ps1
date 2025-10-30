# Pokedex API E2E Test Runner (PowerShell)
# Tests the API running in Docker using local k6 installation

param(
    [string]$Test = "all"
)

$ErrorActionPreference = "Stop"

# Colors
function Write-Info { 
    param([string]$Message)
    Write-Host "[INFO] $Message" -ForegroundColor Blue 
}
function Write-Success { 
    param([string]$Message)
    Write-Host "[SUCCESS] $Message" -ForegroundColor Green 
}
function Write-Error { 
    param([string]$Message)
    Write-Host "[ERROR] $Message" -ForegroundColor Red 
}
function Write-Warning { 
    param([string]$Message)
    Write-Host "[WARNING] $Message" -ForegroundColor Yellow 
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
Write-Host "Pokedex API - E2E Test Runner" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Check if k6 is installed
try {
    $null = Get-Command k6 -ErrorAction Stop
    Write-Success "k6 is installed"
} catch {
    Write-Error "k6 is not installed!"
    Write-Host ""
    Write-Host "Install k6:" -ForegroundColor Yellow
    Write-Host "  Windows (Chocolatey): choco install k6" -ForegroundColor Yellow
    Write-Host "  Windows (Scoop): scoop install k6" -ForegroundColor Yellow
    Write-Host "  Download: https://k6.io/docs/getting-started/installation/" -ForegroundColor Yellow
    exit 1
}

# Check if Docker is running
try {
    docker ps | Out-Null
    Write-Success "Docker is running"
} catch {
    Write-Error "Docker is not running!"
    Write-Host "Please start Docker Desktop" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Info "Building and starting API (Release mode)..."
docker-compose up -d --build pokedex-api

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to start API"
    exit 1
}

Write-Info "Waiting for API to be ready..."
$maxAttempts = 30
$attempt = 0

while ($attempt -lt $maxAttempts) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5000/health" -Method GET -TimeoutSec 2 -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            Write-Host ""
            Write-Success "API is ready!"
            break
        }
    } catch {
        # Continue waiting
    }
    
    $attempt++
    Write-Host "." -NoNewline
    Start-Sleep -Seconds 1
}

if ($attempt -eq $maxAttempts) {
    Write-Host ""
    Write-Error "API failed to start within timeout"
    docker-compose logs pokedex-api
    docker-compose down
    exit 1
}

Write-Host ""
Write-Info "Running functional tests with k6..."
Write-Host ""

$BaseUrl = "http://localhost:5000"
$TestsDir = "k6\tests"
$script:failedTests = 0

# Function to run a test
function Run-Test {
    param(
        [string]$TestName,
        [string]$TestFile,
        [int]$TestNumber,
        [int]$TotalTests
    )
    
    Write-Host ""
    Write-Info "Test $TestNumber/$TotalTests : $TestName"
    Write-Host "----------------------------" -ForegroundColor Gray
    
    & k6 run --env "BASE_URL=$BaseUrl" "$TestsDir\$TestFile"
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "$TestName passed"
        return $true
    } else {
        Write-Warning "$TestName failed"
        $script:failedTests++
        return $false
    }
}

# Run tests based on parameter
switch ($Test.ToLower()) {
    "health" {
        $null = Run-Test "Health Endpoint" "health-test.js" 1 1
    }
    "pokemon" {
        $null = Run-Test "Pokemon Endpoint" "pokemon-test.js" 1 1
    }
    "translated" {
        $null = Run-Test "Translated Pokemon Endpoint" "pokemon-translated-test.js" 1 1
    }
    "all" {
        $null = Run-Test "Health Endpoint" "health-test.js" 1 3
        $null = Run-Test "Pokemon Endpoint" "pokemon-test.js" 2 3
        $null = Run-Test "Translated Pokemon Endpoint" "pokemon-translated-test.js" 3 3
    }
    default {
        Write-Error "Unknown test: $Test"
        Write-Host "Available tests: health, pokemon, translated, all" -ForegroundColor Yellow
        docker-compose down
        exit 1
    }
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan

if ($script:failedTests -eq 0) {
    Write-Success "All tests passed!"
    Write-Host "================================" -ForegroundColor Cyan
} else {
    Write-Warning "$($script:failedTests) test(s) failed"
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Info "This is expected in TDD (RED phase)"
    Write-Info "Implement missing endpoints to make tests pass (GREEN phase)"
}
Write-Host ""
Write-Info "Containers are still running"
Write-Info "Use 'docker-compose down' to stop them"
Write-Host ""