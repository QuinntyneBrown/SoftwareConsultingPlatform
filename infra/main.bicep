@description('The name of the Static Web App')
param staticWebAppName string = 'software-consulting-platform'

@description('The name of the azd environment (e.g. dev, test, prod)')
param environmentName string

@description('The location for the Static Web App')
param location string = resourceGroup().location

@description('The SKU for the Static Web App')
@allowed([
  'Free'
  'Standard'
])
param sku string = 'Free'

@description('Tags for the resources')
param tags object = {
  environment: 'production'
  application: 'software-consulting-platform'
}

var azdTags = {
  'azd-env-name': environmentName
  'azd-service-name': 'web'
}

var mergedTags = union(tags, azdTags)

@description('The repository URL for the application')
param repositoryUrl string = ''

@description('The branch to deploy from')
param branch string = 'main'

@description('The repository token for GitHub/Azure DevOps')
@secure()
param repositoryToken string = ''

module swa 'modules/softwareConsultingPlatformSwa.bicep' = {
  name: 'softwareConsultingPlatformSwaDeployment'
  params: {
    staticWebAppName: staticWebAppName
    location: location
    sku: sku
    tags: mergedTags
    repositoryUrl: repositoryUrl
    branch: branch
    repositoryToken: repositoryToken
  }
}

output staticWebAppId string = swa.outputs.staticWebAppId
output staticWebAppName string = swa.outputs.staticWebAppName
output staticWebAppDefaultHostname string = swa.outputs.staticWebAppDefaultHostname
output staticWebAppUrl string = swa.outputs.staticWebAppUrl
