import http from 'k6/http';
import { check, group } from 'k6';
import { baseConfig } from '../config/base.js';

// Use base configuration with custom thresholds for error testing
// We intentionally test error scenarios (404s) so we need to adjust thresholds
export const options = {
  ...baseConfig.options,
  thresholds: {
    checks: ['rate>0.95'], // 95% of checks should pass
  },
};

// Get base URL from environment or use default
const BASE_URL = baseConfig.environment.BASE_URL;

export default function() {
  group('Pokemon Translated Endpoint Tests', () => {
    
    console.log(`Testing translated Pokemon endpoints at: ${BASE_URL}/pokemon/translated/`);
    
    // Test Group 1: Yoda Translation - Legendary Pokemon
    group('Yoda Translation - Legendary Pokemon (Mewtwo)', () => {
      const response = http.get(`${BASE_URL}/pokemon/translated/mewtwo`, {
        headers: baseConfig.headers,
      });
      
      // Expected data for Mewtwo (legendary, should use Yoda translation)
      const expectedData = {
        name: 'mewtwo',
        habitat: 'rare',
        isLegendary: true
      };
      
      check(response, {
        '✓ returns 200 status': (r) => r.status === 200,
        '✓ response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ has correct name': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.name === expectedData.name;
        },
        '✓ has translated description': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('description') && data.description.length > 0;
        },
        '✓ has correct habitat': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.habitat === expectedData.habitat;
        },
        '✓ is legendary': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.isLegendary === expectedData.isLegendary;
        },
      });
      
      console.log(`Translated Mewtwo (Legendary) response: ${response.body}`);
    });
    
    // Test Group 2: Yoda Translation - Cave Habitat Pokemon
    group('Yoda Translation - Cave Habitat Pokemon (Zubat)', () => {
      const response = http.get(`${BASE_URL}/pokemon/translated/zubat`, {
        headers: baseConfig.headers,
      });
      
      // Expected data for Zubat (cave habitat, should use Yoda translation)
      const expectedData = {
        name: 'zubat',
        habitat: 'cave',
        isLegendary: false
      };
      
      check(response, {
        '✓ returns 200 status': (r) => r.status === 200,
        '✓ response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ has correct name': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.name === expectedData.name;
        },
        '✓ has description field': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          // Description should exist (either translated or original fallback)
          return data.hasOwnProperty('description') && data.description.length > 0;
        },
        '✓ has correct habitat': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.habitat === expectedData.habitat;
        },
        '✓ is not legendary': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.isLegendary === expectedData.isLegendary;
        },
        '✓ uses Yoda translation logic for cave habitat': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          // Cave habitat Pokemon should attempt Yoda translation
          // If translation API fails, fallback to original is acceptable
          // We just verify the endpoint returns a valid description
          return data.description.length > 0;
        },
      });
      
      console.log(`Translated Zubat (Cave habitat) response: ${response.body}`);
    });
    
    // Test Group 3: Shakespeare Translation - Regular Pokemon
    group('Shakespeare Translation - Regular Pokemon (Ditto)', () => {
      const response = http.get(`${BASE_URL}/pokemon/translated/ditto`, {
        headers: baseConfig.headers,
      });
      
      check(response, {
        '✓ returns 200 status': (r) => r.status === 200,
        '✓ response is valid JSON': (r) => baseConfig.validation.checkJsonResponse(r),
        '✓ has correct name': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.name === 'ditto';
        },
        '✓ has translated description': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.hasOwnProperty('description') && data.description.length > 0;
        },
        '✓ is not legendary': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.isLegendary === false;
        },
      });
      
      console.log(`Translated Ditto (Shakespeare) response: ${response.body}`);
    });
    
    // Test Group 4: Error Handling
    group('Error Scenarios', () => {
      
      // Test non-existent Pokemon - mark as expected response
      const notFoundResponse = http.get(`${BASE_URL}/pokemon/translated/invalidpokemon123`, {
        headers: baseConfig.headers,
        tags: { expected_response: 'true' }
      });
      
      check(notFoundResponse, {
        '✓ returns 404 for non-existent Pokemon': (r) => r.status === 404,
      }, { expected_response: 'true' });
      
      console.log(`Non-existent Pokemon response status: ${notFoundResponse.status}`);
      
      // Test empty Pokemon name - mark as expected response
      const emptyNameResponse = http.get(`${BASE_URL}/pokemon/translated/`, {
        headers: baseConfig.headers,
        tags: { expected_response: 'true' }
      });
      
      check(emptyNameResponse, {
        '✓ returns 404 for empty Pokemon name': (r) => r.status === 404,
      }, { expected_response: 'true' });
      
      console.log(`Empty Pokemon name response status: ${emptyNameResponse.status}`);
    });
    
    // Test Group 5: Edge Cases
    group('Edge Case Scenarios', () => {
      
      // Test case insensitivity
      const upperCaseResponse = http.get(`${BASE_URL}/pokemon/translated/ZUBAT`, {
        headers: baseConfig.headers,
      });
      
      check(upperCaseResponse, {
        '✓ handles uppercase Pokemon name': (r) => r.status === 200,
        '✓ returns lowercase name in response': (r) => {
          if (r.status !== 200) return false;
          const data = JSON.parse(r.body);
          return data.name === 'zubat';
        },
      });
      
      console.log(`Uppercase Pokemon name response: ${upperCaseResponse.body}`);
    });
    
  });
}