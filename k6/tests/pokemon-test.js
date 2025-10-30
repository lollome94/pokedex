import http from 'k6/http';
import { check, group } from 'k6';
import { baseConfig } from '../config/base.js';

// Use base configuration - simple functional test
export const options = baseConfig.options;

// Get base URL from environment or use default
const BASE_URL = baseConfig.environment.BASE_URL;

export default function() {
  group('Pokemon Endpoint Tests', () => {
    
    console.log(`Testing Pokemon endpoints at: ${BASE_URL}/pokemon/`);
    
    // Test 1: Pikachu
    group('GET /pokemon/pikachu', () => {
      const response = http.get(`${BASE_URL}/pokemon/pikachu`, {
        headers: baseConfig.headers,
      });
      
      check(response, {
        '✓ pikachu endpoint returns 200': (r) => r.status === 200,
        '✓ pikachu response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ pikachu has name field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('name') && data.name === 'pikachu';
        },
        '✓ pikachu has description field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('description') && data.description.length > 0;
        },
        '✓ pikachu has habitat field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('habitat');
        },
        '✓ pikachu has isLegendary field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('isLegendary') && data.isLegendary === false;
        },
      });
      
      console.log(`Pikachu response: ${response.body}`);
    });
    
    // Test 2: Mewtwo (legendary Pokemon)
    group('GET /pokemon/mewtwo', () => {
      const response = http.get(`${BASE_URL}/pokemon/mewtwo`, {
        headers: baseConfig.headers,
      });
      
      check(response, {
        '✓ mewtwo endpoint returns 200': (r) => r.status === 200,
        '✓ mewtwo response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ mewtwo has name field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('name') && data.name === 'mewtwo';
        },
        '✓ mewtwo has description field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('description') && data.description.length > 0;
        },
        '✓ mewtwo has habitat "rare"': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('habitat') && data.habitat === 'rare';
        },
        '✓ mewtwo is legendary': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('isLegendary') && data.isLegendary === true;
        },
      });
      
      console.log(`Mewtwo response: ${response.body}`);
    });
    
  });
}