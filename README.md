# SoftwareConsultingPlatform

A software consulting platform built with .NET 9.0 and Angular, deployed to Azure.

## CI/CD Pipeline

This repository includes an automated CI/CD pipeline that deploys the application to Azure on every commit to the `main` branch.

### What's Automated

- **Build**: Automatic building of .NET application
- **Test**: Running all tests in the solution
- **Deploy**: Deployment to Azure Static Web Apps using Azure Developer CLI (azd)

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