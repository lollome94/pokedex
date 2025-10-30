import http from 'k6/http';
import { check, group } from 'k6';
import { baseConfig } from '../config/base.js';

// Use base configuration with longer test duration for full suite
export const options = {
  ...baseConfig.options,
  stages: [
    { duration: '15s', target: 1 }, // Warm up
    { duration: '60s', target: 3 }, // Load test all endpoints
    { duration: '15s', target: 0 }, // Cool down
  ],
};

// Get base URL from environment or use default
const BASE_URL = baseConfig.environment.BASE_URL;

export default function() {
  group('Complete API Test Suite', () => {
    
    // Test 1: Health Check
    group('Health Endpoint', () => {
      const healthResponse = http.get(`${BASE_URL}/health`);
      check(healthResponse, {
        'health endpoint is available': (r) => r.status === 200,
      });
    });
    
    // Test 2: Pokemon Endpoint
    group('Pokemon Endpoints', () => {
      const pokemonName = 'pikachu';
      
      // Regular Pokemon endpoint
      const pokemonResponse = http.get(`${BASE_URL}/pokemon/${pokemonName}`);
      check(pokemonResponse, {
        'pokemon endpoint is available': (r) => r.status === 200,
        'pokemon returns valid data': (r) => {
          if (r.status === 200) {
            const data = JSON.parse(r.body);
            return data.name && data.description && data.habitat !== undefined && data.isLegendary !== undefined;
          }
          return false;
        },
      });
      
      // Translated Pokemon endpoint
      const translatedResponse = http.get(`${BASE_URL}/pokemon/translated/${pokemonName}`);
      check(translatedResponse, {
        'translated pokemon endpoint is available': (r) => r.status === 200,
        'translated pokemon returns valid data': (r) => {
          if (r.status === 200) {
            const data = JSON.parse(r.body);
            return data.name && data.description && data.habitat !== undefined && data.isLegendary !== undefined;
          }
          return false;
        },
      });
    });
    
    // Test 3: Error Handling
    group('Error Handling', () => {
      // Test 404 for non-existent Pokemon
      const notFoundResponse = http.get(`${BASE_URL}/pokemon/nonexistentpokemon`);
      check(notFoundResponse, {
        'non-existent pokemon returns proper error': (r) => r.status === 404 || r.status === 400,
      });
      
      // Test 404 for non-existent endpoint
      const invalidEndpointResponse = http.get(`${BASE_URL}/invalid-endpoint`);
      check(invalidEndpointResponse, {
        'invalid endpoint returns proper error': (r) => r.status === 404,
      });
    });
    
    // Test 4: API Performance
    group('Performance Tests', () => {
      const startTime = new Date();
      
      // Make multiple requests
      const responses = http.batch({
        'health': ['GET', `${BASE_URL}/health`],
        'pikachu': ['GET', `${BASE_URL}/pokemon/pikachu`],
        'mewtwo': ['GET', `${BASE_URL}/pokemon/mewtwo`],
        'translated_pikachu': ['GET', `${BASE_URL}/pokemon/translated/pikachu`],
      });
      
      const endTime = new Date();
      const totalTime = endTime - startTime;
      
      check(responses, {
        'all requests complete within acceptable time': () => totalTime < 2000, // 2 seconds
        'all endpoints respond successfully': (responses) => {
          return Object.values(responses).every(r => r.status === 200 || r.status === 404);
        },
      });
    });
    
  });
}