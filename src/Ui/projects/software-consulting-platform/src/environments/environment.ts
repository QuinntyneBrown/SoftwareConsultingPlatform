export const environment = {
  production: false,
  // API Gateway URL - uses proxy in development, routes requests to appropriate microservices
  baseUrl: '/api',
  // Feature flags for microservices migration
  useMicroservices: true,
  // Retry configuration
  retryConfig: {
    maxRetries: 3,
    retryDelayMs: 1000,
    retryStatusCodes: [500, 502, 503, 504]
  }
};
