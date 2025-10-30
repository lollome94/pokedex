#!/bin/bash
# Pokedex API - E2E Test Runner (macOS/Linux)
# Tests the API running in Docker using local k6 installation

set -e

# Parse arguments
TEST="${1:-all}"

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

echo ""
echo "================================"
echo "Pokedex API - E2E Test Runner"
echo "================================"
echo ""

# Check if k6 is installed
if ! command -v k6 &> /dev/null; then
    print_error "k6 is not installed!"
    echo ""
    echo "Install k6:"
    echo "  macOS (Homebrew): brew install k6"
    echo "  Linux (Debian/Ubuntu): sudo gpg -k && sudo gpg --no-default-keyring --keyring /usr/share/keyrings/k6-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69 && echo 'deb [signed-by=/usr/share/keyrings/k6-archive-keyring.gpg] https://dl.k6.io/deb stable main' | sudo tee /etc/apt/sources.list.d/k6.list && sudo apt-get update && sudo apt-get install k6"
    echo "  Download: https://k6.io/docs/getting-started/installation/"
    exit 1
fi

print_success "k6 is installed"

# Check if Docker is running
if ! docker ps &> /dev/null; then
    print_error "Docker is not running!"
    echo "Please start Docker"
    exit 1
fi

print_success "Docker is running"

echo ""
print_info "Building and starting API (Release mode)..."
docker-compose up -d --build pokedex-api

if [ $? -ne 0 ]; then
    print_error "Failed to start API"
    exit 1
fi

print_info "Waiting for API to be ready..."
max_attempts=30
attempt=0

while [ $attempt -lt $max_attempts ]; do
    if curl -s -f http://localhost:5000/health > /dev/null 2>&1; then
        echo ""
        print_success "API is ready!"
        break
    fi
    
    attempt=$((attempt + 1))
    echo -n "."
    sleep 1
done

if [ $attempt -eq $max_attempts ]; then
    echo ""
    print_error "API failed to start within timeout"
    docker-compose logs pokedex-api
    docker-compose down
    exit 1
fi

echo ""
print_info "Running functional tests with k6..."
echo ""

BASE_URL="http://localhost:5000"
TESTS_DIR="k6/tests"
failed_tests=0

# Function to run a test
run_test() {
    local test_name="$1"
    local test_file="$2"
    local test_number="$3"
    local total_tests="$4"
    
    echo ""
    print_info "Test $test_number/$total_tests: $test_name"
    echo "----------------------------"
    
    if k6 run --env "BASE_URL=$BASE_URL" "$TESTS_DIR/$test_file"; then
        print_success "$test_name passed"
        return 0
    else
        print_warning "$test_name failed"
        return 1
    fi
}

# Run tests based on parameter
case "$TEST" in
    health)
        run_test "Health Endpoint" "health-test.js" 1 1 || ((failed_tests++))
        ;;
    pokemon)
        run_test "Pokemon Endpoint" "pokemon-test.js" 1 1 || ((failed_tests++))
        ;;
    translated)
        run_test "Translated Pokemon Endpoint" "pokemon-translated-test.js" 1 1 || ((failed_tests++))
        ;;
    all)
        run_test "Health Endpoint" "health-test.js" 1 3 || ((failed_tests++))
        run_test "Pokemon Endpoint" "pokemon-test.js" 2 3 || ((failed_tests++))
        run_test "Translated Pokemon Endpoint" "pokemon-translated-test.js" 3 3 || ((failed_tests++))
        ;;
    *)
        print_error "Unknown test: $TEST"
        echo "Available tests: health, pokemon, translated, all"
        docker-compose down
        exit 1
        ;;
esac

echo ""
echo "================================"

if [ $failed_tests -eq 0 ]; then
    print_success "All tests passed!"
    echo "================================"
else
    print_warning "$failed_tests test(s) failed"
    echo "================================"
    echo ""
    print_info "This is expected in TDD (RED phase)"
    print_info "Implement missing endpoints to make tests pass (GREEN phase)"
fi

echo ""
print_info "Containers are still running"
print_info "Use 'docker-compose down' to stop them"
echo ""