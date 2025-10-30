// Base configuration for k6 functional tests
export const baseConfig = {
  // Simple functional test options - no load testing
  options: {
    vus: 1,           // Single virtual user
    iterations: 1,    // Run once
    thresholds: {
      http_req_failed: ['rate<0.01'], // Less than 1% failure rate
    },
  },
  
  // Environment configuration
  environment: {
    // API running in Docker, exposed on localhost:5000
    BASE_URL: __ENV.BASE_URL || 'http://localhost:5000',
  },
  
  // Common headers for all requests
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
  
  // Response validation helpers
  validation: {
    // Check if response has expected status code
    checkStatus: (response, expectedStatus = 200) => {
      return response.status === expectedStatus;
    },
    
    // Check if response body is valid JSON
    checkJsonResponse: (response) => {
      try {
        JSON.parse(response.body);
        return true;
      } catch (e) {
        return false;
      }
    },
    
    // Check if response has required fields
    checkRequiredFields: (responseBody, requiredFields) => {
      const data = JSON.parse(responseBody);
      return requiredFields.every(field => data.hasOwnProperty(field));
    },
  },
};