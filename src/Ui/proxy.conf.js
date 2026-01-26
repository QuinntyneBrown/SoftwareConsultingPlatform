const PROXY_CONFIG = [
  {
    context: ['/api'],
    target: process.env['services__api-gateway__https__0'] || process.env['services__api-gateway__http__0'] || 'http://localhost:5000',
    secure: false,
    changeOrigin: true,
    logLevel: 'debug'
  }
];

module.exports = PROXY_CONFIG;
