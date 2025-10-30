import http from 'k6/http';
import { check, group } from 'k6';
import { baseConfig } from '../config/base.js';

// Use base configuration - simple functional test
export const options = baseConfig.options;

// Get base URL from environment or use default
const BASE_URL = baseConfig.environment.BASE_URL;

export default function() {
  group('Health Endpoint Tests', () => {
    
    console.log(`Testing health endpoint at: ${BASE_URL}/health`);
    
    const response = http.get(`${BASE_URL}/health`, {
      headers: baseConfig.headers,
    });
    
    // Test 1: Should return 200 OK
    check(response, {
      '✓ health endpoint returns 200': (r) => r.status === 200,
    });
    
    // Test 2: Should return valid JSON
    check(response, {
      '✓ health response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
    });
    
    // Test 3: Should have required fields
    check(response, {
      '✓ health response has status field': (r) => {
        const data = JSON.parse(r.body);
        return data.hasOwnProperty('status');
      },
      '✓ health response has timestamp field': (r) => {
        const data = JSON.parse(r.body);
        return data.hasOwnProperty('timestamp');
      },
    });
    
    // Test 4: Should have correct field values
    check(response, {
      '✓ health status is "healthy"': (r) => {
        const data = JSON.parse(r.body);
        return data.status === 'healthy';
      },
      '✓ health timestamp is valid ISO date': (r) => {
        const data = JSON.parse(r.body);
        const timestamp = new Date(data.timestamp);
        return !isNaN(timestamp.getTime());
      },
    });
    
    // Log response for debugging
    console.log(`Response: ${response.body}`);
  });
}