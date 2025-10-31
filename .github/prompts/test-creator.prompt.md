---
mode: agent
---

# 🧪 E2E Test Creator - Copilot Instructions

## Purpose
Create new k6 E2E tests for the Pokedex API following established patterns and best practices.

## Usage
```
@workspace Create E2E test for [endpoint-name] testing [functionality]
@workspace Add test case to [existing-test] for [scenario]
@workspace Create comprehensive test suite for [feature]
```

## Parameters
- **endpoint-name**: The API endpoint to test (e.g., `/pokemon/{name}`, `/health`)
- **functionality**: What to test (e.g., "error handling", "validation", "performance")
- **scenario**: Specific test scenario (e.g., "non-existent Pokemon", "special characters")
- **feature**: Feature name for comprehensive testing

## Test Creation Steps

### 1. Analyze Requirements
Before creating the test:
- ✅ **Understand endpoint**: Review endpoint implementation in `Features/` folder
- ✅ **Identify test cases**: Determine happy path, edge cases, error scenarios
- ✅ **Check existing tests**: Review similar tests in `k6/tests/` for patterns
- ✅ **Verify base config**: Understand configuration in `k6/config/base.js`

### 2. Determine Test Type
Choose the appropriate test type:

| Type | When to Use | File Pattern |
|------|-------------|--------------|
| **Unit Test** | Single endpoint, specific functionality | `[endpoint-name]-test.js` |
| **Integration Test** | Multiple endpoints, feature flow | `[feature-name]-integration-test.js` |
| **Validation Test** | Input validation, error handling | `[endpoint-name]-validation-test.js` |
| **Performance Test** | Load testing, stress testing | `[endpoint-name]-load-test.js` |

### 3. Create Test File
Generate the test file in `k6/tests/` following this structure:

```javascript
import http from 'k6/http';
import { check, group } from 'k6';
import { baseConfig } from '../config/base.js';

// Use base configuration
export const options = baseConfig.options;

// Get base URL from environment
const BASE_URL = baseConfig.environment.BASE_URL;

export default function() {
  group('[Feature Name] Tests', () => {
    
    console.log(`Testing [endpoint] at: ${BASE_URL}/[endpoint]/`);
    
    // Test Group 1: Happy Path
    group('[Scenario Description]', () => {
      const response = http.get(`${BASE_URL}/[endpoint]/[param]`, {
        headers: baseConfig.headers,
      });
      
      // Define expected data
      const expectedData = {
        field1: "value1",
        field2: "value2"
      };
      
      // Assertions
      check(response, {
        '✓ returns 200 status': (r) => r.status === 200,
        '✓ response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ has expected field1': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.field1 === expectedData.field1;
        },
        '✓ has expected field2': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.field2 === expectedData.field2;
        }
      });
      
      console.log(`Response: ${response.body}`);
    });
    
    // Test Group 2: Error Handling
    group('Error Scenarios', () => {
      const response = http.get(`${BASE_URL}/[endpoint]/invalid-input`, {
        headers: baseConfig.headers,
      });
      
      check(response, {
        '✓ returns 404 for invalid input': (r) => r.status === 404,
        '✓ error response is JSON': (r) => baseConfig.validation.checkJsonResponse(r)
      });
    });
    
  });
}
```

### 4. Define Test Scenarios
For each test file, include these scenario categories:

#### ✅ Happy Path Tests
- Valid inputs with expected outputs
- Standard use cases
- Common user workflows

#### 🔍 Edge Case Tests
- Boundary values (empty strings, very long inputs)
- Special characters
- Unicode characters
- Case sensitivity

#### ❌ Error Handling Tests
- Invalid inputs (non-existent resources)
- Malformed requests
- Unsupported methods
- Missing required parameters

#### ⚡ Performance Tests (Optional)
- Response time validation
- Concurrent request handling
- Rate limiting behavior

### 5. Create HTTP Test File
For manual testing, create corresponding `.http` file in the Feature folder:

```http
### Test: [Scenario Name]
GET http://localhost:8080/[endpoint]/[param]
Accept: application/json

### Test: [Error Scenario]
GET http://localhost:8080/[endpoint]/invalid
Accept: application/json
```

### 6. Update Test Suite
Add the new test to `full-suite-test.js` if applicable:

```javascript
import { group } from 'k6';

// Import new test
import newTestFunction from './new-test.js';

export default function() {
  group('Full Test Suite', () => {
    // ... existing tests
    
    group('New Test Suite', newTestFunction);
  });
}
```

### 7. Document the Test
Add test documentation to README.md in the **Testing** section:

```markdown
### [Test Name]
**File**: `k6/tests/[test-file].js`  
**Purpose**: Tests [functionality description]  
**Scenarios**:
- ✅ Happy path: [description]
- 🔍 Edge cases: [description]
- ❌ Error handling: [description]

**Run**: `.\run-e2e-tests.ps1 -Test [test-name]`
```

## Test Patterns & Best Practices

### ✅ DO:
- **Use descriptive test names** with ✓ prefix: `'✓ returns valid Pokemon data'`
- **Group related tests** using `group()` for better organization
- **Log responses** for debugging: `console.log()`
- **Validate response structure** not just status codes
- **Use exact value matching** for critical fields
- **Include console output** to show what's being tested
- **Follow existing patterns** from `pokemon-test.js` and `health-test.js`
- **Test both success and failure paths**

### ❌ DON'T:
- Hardcode URLs (use `BASE_URL` from config)
- Skip error scenario testing
- Use vague assertion messages
- Create tests without corresponding `.http` files
- Ignore response validation
- Test only happy paths
- Use complex load testing scenarios for functional tests

## Example Test Scenarios

### Example 1: Create Validation Test
```
User: @workspace Create E2E test for Pokemon endpoint testing special characters in names

Required Test Cases:
1. Pokemon with hyphens (e.g., "ho-oh")
2. Pokemon with spaces (should return 404)
3. Pokemon with special characters (should return 404)
4. Case insensitivity (e.g., "Mewtwo" vs "mewtwo")
```

### Example 2: Add Error Handling Test
```
User: @workspace Add test case to pokemon-test for non-existent Pokemon

Add to existing pokemon-test.js:
- Test with completely invalid Pokemon name
- Verify 404 status code
- Verify error response format
- Add descriptive check messages
```

### Example 3: Create Performance Test
```
User: @workspace Create performance test for translated endpoint with concurrent requests

Create pokemon-translated-load-test.js:
- Configure for 10 VUs (virtual users)
- Duration: 30 seconds
- Test multiple Pokemon in parallel
- Measure response times
- Validate all responses succeed
```

## Test File Naming Convention

| Pattern | Example | Purpose |
|---------|---------|---------|
| `[endpoint]-test.js` | `pokemon-test.js` | Main functional test |
| `[endpoint]-validation-test.js` | `pokemon-validation-test.js` | Input validation tests |
| `[endpoint]-error-test.js` | `pokemon-error-test.js` | Error scenario tests |
| `[endpoint]-load-test.js` | `pokemon-load-test.js` | Performance/load tests |
| `[feature]-integration-test.js` | `translation-integration-test.js` | Multi-endpoint tests |

## Checklist for New Test

Before completing test creation, verify:

- [ ] Test file created in `k6/tests/`
- [ ] Follows existing test structure and patterns
- [ ] Uses `baseConfig` from `../config/base.js`
- [ ] Includes happy path scenarios
- [ ] Includes error handling scenarios
- [ ] Includes edge case testing
- [ ] Has descriptive check messages with ✓ prefix
- [ ] Logs responses for debugging
- [ ] Corresponding `.http` file created in Feature folder
- [ ] Test documented in README.md
- [ ] Test can be run via `run-e2e-tests.ps1/sh`
- [ ] Test added to `full-suite-test.js` (if applicable)

## Success Criteria
- ✅ Test follows established patterns
- ✅ Comprehensive scenario coverage
- ✅ Clear, descriptive assertions
- ✅ Both happy and error paths tested
- ✅ Documentation updated
- ✅ Test can be executed successfully
- ✅ Results are clear and actionable

## Integration with Architecture

### Vertical Slice Architecture Alignment
When creating tests:
1. **Mirror Feature structure**: Test organization should match `Features/` folder structure
2. **Test complete slice**: Test request → endpoint → response flow
3. **Validate contracts**: Verify request/response DTOs match Contracts.cs
4. **Test validation rules**: Cover all FluentValidation scenarios

### Example Feature-Test Mapping
```
Features/Pokemon/GetPokemon/
├── GetPokemon.Contracts.cs
├── GetPokemon.Endpoint.cs
├── GetPokemon.Validator.cs
└── GetPokemon.http

k6/tests/
└── pokemon-test.js  ← Tests this entire slice
```

## Notes
- All tests should be functional E2E tests, not unit tests
- Tests run against Dockerized API (`http://localhost:8080`)
- Use k6 check() for assertions, not k6/expect
- Keep tests fast (< 30s execution time)
- Focus on API contract validation
- Follow English-first standard (comments, messages)
