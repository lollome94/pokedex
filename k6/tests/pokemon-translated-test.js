import http from 'k6/http';
import { check, group } from 'k6';
import { baseConfig } from '../config/base.js';

// Use base configuration - simple functional test
export const options = baseConfig.options;

// Get base URL from environment or use default
const BASE_URL = baseConfig.environment.BASE_URL;

export default function() {
  group('Pokemon Translated Endpoint Tests', () => {
    
    console.log(`Testing translated Pokemon endpoints at: ${BASE_URL}/pokemon/translated/`);
    
    // Test Mewtwo translation
    group('GET /pokemon/translated/mewtwo', () => {
      const response = http.get(`${BASE_URL}/pokemon/translated/mewtwo`, {
        headers: baseConfig.headers,
      });
      
      check(response, {
        '✓ translated mewtwo endpoint returns 200': (r) => r.status === 200,
        '✓ translated mewtwo response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ translated mewtwo has name field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('name') && data.name === 'mewtwo';
        },
        '✓ translated mewtwo has description field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('description') && data.description.length > 0;
        },
        '✓ translated mewtwo has habitat field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('habitat') && data.habitat === 'rare';
        },
        '✓ translated mewtwo is legendary': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('isLegendary') && data.isLegendary === true;
        },
      });
      
      console.log(`Translated Mewtwo response: ${response.body}`);
    });
    
  });
}