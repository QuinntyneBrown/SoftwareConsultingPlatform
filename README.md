# SoftwareConsultingPlatform

## Deploy to Azure (azd)

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