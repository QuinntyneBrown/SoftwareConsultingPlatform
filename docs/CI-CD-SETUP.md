# CI/CD Setup Guide

This guide explains how to set up the CI/CD pipeline for the SoftwareConsultingPlatform application.

## Overview

The CI/CD pipeline is configured using GitHub Actions and deploys the application to Azure on each commit to the `main` branch. The workflow consists of two jobs:

1. **Build and Test**: Builds the .NET application and runs tests
2. **Deploy**: Deploys the application to Azure using Azure Developer CLI (azd)

## Prerequisites

- Azure subscription
- Azure CLI (`az`)
- Azure Developer CLI (`azd`)
- Appropriate permissions to create resources in Azure

## Required GitHub Configuration

### GitHub Secrets and Variables

You need to configure the following in your GitHub repository under **Settings → Secrets and variables → Actions**:

#### Option 1: Using Federated Credentials (Recommended)

**Variables** (Repository variables):
- `AZURE_CLIENT_ID` - The client ID of your Azure service principal
- `AZURE_TENANT_ID` - Your Azure tenant ID
- `AZURE_ENV_NAME` - Name for your Azure environment (e.g., `dev`, `prod`)
- `AZURE_LOCATION` - Azure region (e.g., `eastus`, `westus2`)
- `AZURE_SUBSCRIPTION_ID` - Your Azure subscription ID

#### Option 2: Using Client Credentials

**Secrets** (Repository secrets):
- `AZURE_CREDENTIALS` - JSON containing Azure service principal credentials:
  ```json
  {
    "clientId": "your-client-id",
    "clientSecret": "your-client-secret",
    "tenantId": "your-tenant-id"
  }
  ```

**Variables** (Repository variables):
- `AZURE_ENV_NAME` - Name for your Azure environment (e.g., `dev`, `prod`)
- `AZURE_LOCATION` - Azure region (e.g., `eastus`, `westus2`)
- `AZURE_SUBSCRIPTION_ID` - Your Azure subscription ID

### GitHub Environment

The workflow uses a `production` environment. You may need to configure this in **Settings → Environments**:

1. Create an environment named `production`
2. Optionally add protection rules (e.g., required reviewers)

## Setting Up Azure Service Principal

### For Federated Credentials (Recommended)

1. Create a service principal:
   ```bash
   az ad sp create-for-rbac --name "software-consulting-platform-sp" --role contributor \
     --scopes /subscriptions/{subscription-id} \
     --json-auth
   ```

2. Configure federated credentials for GitHub Actions:
   ```bash
   az ad app federated-credential create \
     --id <app-id> \
     --parameters '{
       "name": "github-actions-deploy",
       "issuer": "https://token.actions.githubusercontent.com",
       "subject": "repo:QuinntyneBrown/SoftwareConsultingPlatform:environment:production",
       "audiences": ["api://AzureADTokenExchange"]
     }'
   ```

3. Add the values to GitHub as variables (see above)

### For Client Credentials

1. Create a service principal and get credentials:
   ```bash
   az ad sp create-for-rbac --name "software-consulting-platform-sp" --role contributor \
     --scopes /subscriptions/{subscription-id} \
     --sdk-auth
   ```

2. Copy the JSON output and add it as the `AZURE_CREDENTIALS` secret in GitHub

## Local Deployment Testing

To test deployment locally before pushing to `main`:

1. Install prerequisites:
   ```bash
   # Install Azure CLI
   curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
   
   # Install Azure Developer CLI
   curl -fsSL https://aka.ms/install-azd.sh | bash
   ```

2. Login to Azure:
   ```bash
   az login
   azd auth login
   ```

3. Deploy:
   ```bash
   azd up
   ```

## Workflow Triggers

The workflow is triggered by:
- **Push to `main` branch**: Automatically builds and deploys
- **Manual trigger**: Can be triggered manually from the GitHub Actions tab using "Run workflow"

## Workflow Jobs

### Build and Test Job

- Checks out the code
- Sets up .NET 9.0
- Restores NuGet packages
- Builds the solution in Release configuration
- Runs tests (continues even if tests fail)

### Deploy Job

- Checks out the code
- Sets up Azure Developer CLI
- Authenticates with Azure (using either federated or client credentials)
- Provisions infrastructure and deploys application using `azd up`

## Infrastructure

The application infrastructure is defined in `infra/main.bicep` and includes:
- Azure Static Web App for hosting the Angular frontend

The deployment is configured in `azure.yaml` which specifies:
- Service name and type
- Build hooks
- Output locations

## Troubleshooting

### Workflow Fails with Authentication Error

- Verify that all required secrets/variables are set correctly in GitHub
- Check that the service principal has the necessary permissions
- For federated credentials, ensure the subject matches your repository and environment

### Build Fails

- Check that the .NET SDK version matches the project requirements (currently 9.0)
- Verify all dependencies are properly restored
- Review the build logs in the GitHub Actions run

### Deployment Fails

- Verify the Azure subscription has enough quota for the resources
- Check that the resource names don't conflict with existing resources
- Review the azd logs in the GitHub Actions run

## Additional Resources

- [Azure Developer CLI Documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Azure Static Web Apps Documentation](https://learn.microsoft.com/azure/static-web-apps/)
