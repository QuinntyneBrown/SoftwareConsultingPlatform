export const environment = {
  production: true,
  // API Gateway URL - set by deployment pipeline
  baseUrl: '/api',
  // Legacy monolith URL (not used in production after migration)
  legacyApiUrl: '',
  // Feature flags for microservices migration
  useMicroservices: true,
  // Retry configuration
  retryConfig: {
    maxRetries: 3,
    retryDelayMs: 1000,
    retryStatusCodes: [500, 502, 503, 504]
  }
};
