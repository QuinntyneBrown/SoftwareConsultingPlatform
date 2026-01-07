@description('The name of the Static Web App')
param staticWebAppName string

@description('The location for the Static Web App')
param location string

@description('The SKU for the Static Web App')
@allowed([
  'Free'
  'Standard'
])
param sku string

@description('Tags for the resources')
param tags object

@description('The repository URL for the application')
param repositoryUrl string

@description('The branch to deploy from')
param branch string

@description('The repository token for GitHub/Azure DevOps')
@secure()
param repositoryToken string

var useRepoIntegration = !empty(repositoryUrl) && !empty(repositoryToken)

var repoIntegrationProps = useRepoIntegration ? {
  repositoryUrl: repositoryUrl
  branch: branch
  repositoryToken: repositoryToken
  provider: 'GitHub'
} : {}

resource staticWebApp 'Microsoft.Web/staticSites@2023-01-01' = {
  name: staticWebAppName
  location: location
  tags: tags
  sku: {
    name: sku
    tier: sku
  }
  properties: union({
    buildProperties: {
      appLocation: 'src/SoftwareConsultingPlatform.WebApp'
      apiLocation: ''
      outputLocation: 'dist/software-consulting-platform/browser'
      appBuildCommand: 'npm run build'
      apiBuildCommand: ''
      skipGithubActionWorkflowGeneration: !useRepoIntegration
    }
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true
    enterpriseGradeCdnStatus: 'Disabled'
  }, repoIntegrationProps)
}

output staticWebAppId string = staticWebApp.id
output staticWebAppName string = staticWebApp.name
output staticWebAppDefaultHostname string = staticWebApp.properties.defaultHostname
output staticWebAppUrl string = 'https://${staticWebApp.properties.defaultHostname}'
