---
mode: agent
---
# 🧪 E2E Test Runner - Copilot Instructions

## Purpose
Execute k6 E2E tests for the Pokedex API with automatic platform detection, test execution, and optional README status updates.

## Usage
```
@workspace Run E2E tests for [test-name]
@workspace Run all E2E tests
@workspace Run E2E tests for [test-name] and update README with results
```

## Parameters
- **test-name**: The test to run (options: `all`, `health`, `pokemon`, `pokemon-translated`, `full-suite`)
- **update-readme**: Optional flag to update README.md with test results

## Execution Steps

### 1. Platform Detection
Detect the user's operating system:
- **Windows**: Use `run-e2e-tests.ps1`
- **macOS/Linux**: Use `run-e2e-tests.sh`

### 2. Prerequisites Check
Before running tests, verify:
- ✅ **k6 installed**: Check if k6 is available in PATH
- ✅ **Docker running**: Verify Docker daemon is active
- ✅ **Test file exists**: Confirm the requested test file exists in `k6/tests/`

If any prerequisite fails:
- Provide clear installation instructions
- Do NOT proceed with test execution

### 3. Execute Test Script
Run the appropriate script based on platform:

**Windows (PowerShell):**
```powershell
.\run-e2e-tests.ps1 -Test <test-name>
```

**macOS/Linux (Bash):**
```bash
./run-e2e-tests.sh <test-name>
```

### 4. Capture Test Results
Monitor the test execution and capture:
- ✅ Test status (passed/failed)
- 📊 Number of checks passed/failed
- ⏱️ Execution time
- 🔍 Any error messages or warnings

### 5. Optional: Update README
If user requested README update, modify the **Test Results** section in README.md:

**Format:**
```markdown
## 🧪 Latest Test Results

**Last Run**: [Date and Time]  
**Platform**: [Windows/macOS/Linux]

| Test Suite | Status | Checks | Duration |
|------------|--------|--------|----------|
| Health Check | ✅ PASSED | 5/5 | 2.3s |
| Pokemon Endpoint | ✅ PASSED | 15/15 | 4.1s |
| Pokemon Translated | ✅ PASSED | 18/18 | 5.8s |
| Full Suite | ✅ PASSED | 38/38 | 12.2s |

**Notes**: All tests passed successfully.
```

### 6. Provide User Feedback
After execution, provide:
- ✅ Clear summary of results (passed/failed)
- 📝 Any issues encountered
- 💡 Suggestions if tests failed
- 🔗 Link to full test output if available

## Available Tests

| Test Name | File | Description |
|-----------|------|-------------|
| `health` | `health-test.js` | Tests the `/health` endpoint |
| `pokemon` | `pokemon-test.js` | Tests the `/pokemon/{name}` endpoint |
| `pokemon-translated` | `pokemon-translated-test.js` | Tests the `/pokemon/translated/{name}` endpoint |
| `full-suite` | `full-suite-test.js` | Runs all tests in a comprehensive suite |
| `all` | All test files | Executes all individual test files sequentially |

## Error Handling

### Common Issues & Solutions

**k6 not found:**
```
Windows: choco install k6 or scoop install k6
macOS: brew install k6
Linux: Follow instructions at https://k6.io/docs/getting-started/installation/
```

**Docker not running:**
```
Start Docker Desktop and wait for it to be fully ready
Verify with: docker ps
```

**Test file not found:**
```
Check k6/tests/ directory for available tests
Verify the test name is spelled correctly
```

**API not responding:**
```
1. Rebuild Docker image: docker-compose build
2. Restart containers: docker-compose down && docker-compose up -d
3. Check logs: docker-compose logs -f
4. Wait 10-15 seconds for API to be fully ready
```

## Example Interactions

### Example 1: Run Single Test
```
User: @workspace Run E2E tests for pokemon
Assistant: 
1. Detected Windows platform, using run-e2e-tests.ps1
2. Prerequisites checked: k6 ✅, Docker ✅
3. Executing: .\run-e2e-tests.ps1 -Test pokemon
4. Results: ✅ PASSED - 15/15 checks passed in 4.1s
```

### Example 2: Run All Tests with README Update
```
User: @workspace Run all E2E tests and update README with results
Assistant:
1. Detected macOS platform, using run-e2e-tests.sh
2. Prerequisites checked: k6 ✅, Docker ✅
3. Executing all test suites...
4. Results: ✅ ALL PASSED - 38/38 checks passed
5. README.md updated with latest test results
```

### Example 3: Handle Prerequisites Failure
```
User: @workspace Run E2E tests for health
Assistant:
❌ Cannot run tests - k6 is not installed.

Install k6 on Windows:
- Chocolatey: choco install k6
- Scoop: scoop install k6
- Download: https://k6.io/docs/getting-started/installation/

After installation, try again.
```

## Success Criteria
- ✅ Correct script executed based on platform
- ✅ Prerequisites verified before execution
- ✅ Test results clearly communicated
- ✅ README updated if requested
- ✅ Helpful error messages if issues occur
- ✅ No manual intervention required from user

## Notes
- Always use the existing `run-e2e-tests.ps1` or `run-e2e-tests.sh` scripts
- Do NOT create new test runner scripts
- Capture full terminal output for troubleshooting
- Tests run against Dockerized API on `http://localhost:8080`
- Default test timeout is 30 seconds per endpoint
