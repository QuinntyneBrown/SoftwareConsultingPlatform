# SoftwareConsultingPlatform

A software consulting platform with an Angular web application, deployed to Azure Static Web Apps.

## CI/CD Pipeline

This repository includes an automated CI/CD pipeline that deploys the Angular application to Azure Static Web Apps on every commit to the `main` branch.

### What's Automated

- **Deploy**: Automatic deployment of the Angular application to Azure Static Web Apps using Azure Developer CLI (azd)
- **Infrastructure**: Automatic provisioning of Azure resources using Bicep templates

### Setup Requirements

To enable automated deployments, configure the following in your GitHub repository:

**Required Variables** (Settings → Secrets and variables → Actions → Variables):
- `AZURE_CLIENT_ID` - Azure service principal client ID
- `AZURE_TENANT_ID` - Azure tenant ID
- `AZURE_ENV_NAME` - Environment name (e.g., `prod`)
- `AZURE_LOCATION` - Azure region (e.g., `eastus`)
- `AZURE_SUBSCRIPTION_ID` - Azure subscription ID

**OR**

**Required Secrets** (Settings → Secrets and variables → Actions → Secrets):
- `AZURE_CREDENTIALS` - JSON with Azure service principal credentials

For detailed setup instructions, see [CI/CD Setup Guide](docs/CI-CD-SETUP.md).

## Manual Deploy to Azure (azd)

This repo is configured to deploy the Angular app (`src/SoftwareConsultingPlatform.WebApp`) to **Azure Static Web Apps** using the Azure Developer CLI (`azd`) and the Bicep in `infra/main.bicep`.

### Prereqs

- Azure CLI (`az`) and Azure Developer CLI (`azd`)
- Node.js + npm

### Deploy

From the repo root:

1. Login:
	- `az login`
	- `azd auth login`
2. Create an environment and deploy:
	- `azd up`

`azd` provisions infrastructure from `infra/main.bicep` and builds the Angular app via `infra/scripts/build-web.ps1`.