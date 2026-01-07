# SoftwareConsultingPlatform

A software consulting platform with an Angular web application, deployed to Azure Static Web Apps.

## CI/CD Pipeline

This repository includes an automated CI/CD pipeline that deploys the Angular application to Azure Static Web Apps on every commit to the `main` branch.

### What's Automated

- **Build**: Automatic build of the Angular application using Node.js and npm
- **Deploy**: Automatic deployment to Azure Static Web Apps using the deployment token

### Setup Requirements

To enable automated deployments, configure the following in your GitHub repository:

**Required Secrets** (Settings → Secrets and variables → Actions → Secrets):
- `AZURE_STATIC_WEB_APPS_API_TOKEN` - Deployment token for the Azure Static Web Apps resource

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