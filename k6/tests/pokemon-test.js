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
    
    // Test: Mewtwo (legendary Pokemon)
    group('GET /pokemon/mewtwo', () => {
      const response = http.get(`${BASE_URL}/pokemon/mewtwo`, {
        headers: baseConfig.headers,
      });
      
      const expectedMewtwoData = {
        name: "mewtwo",
        description: "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
        habitat: "rare",
        isLegendary: true
      };
      
      check(response, {
        '✓ mewtwo endpoint returns 200': (r) => r.status === 200,
        '✓ mewtwo response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ mewtwo has exact name': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.name === expectedMewtwoData.name;
        },
        '✓ mewtwo has exact description': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.description === expectedMewtwoData.description;
        },
        '✓ mewtwo has exact habitat': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.habitat === expectedMewtwoData.habitat;
        },
        '✓ mewtwo is legendary': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.isLegendary === expectedMewtwoData.isLegendary;
        },
        '✓ mewtwo response matches expected structure': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.name === expectedMewtwoData.name &&
                 data.description === expectedMewtwoData.description &&
                 data.habitat === expectedMewtwoData.habitat &&
                 data.isLegendary === expectedMewtwoData.isLegendary;
        },
      });
      
      console.log(`Mewtwo response: ${response.body}`);
    });
    
  });
}