# CI/CD Setup Guide

This guide explains how to set up the CI/CD pipeline for the SoftwareConsultingPlatform application.

## Overview

The CI/CD pipeline is configured using GitHub Actions and deploys the Angular application to Azure Static Web Apps on each commit to the `main` branch. The workflow uses the Azure Static Web Apps deployment action with a deployment token.

## Prerequisites

- Azure subscription with an Azure Static Web Apps resource already created
- Access to the Azure Portal to retrieve the deployment token
- Node.js 20 (for the build process)
- npm package manager

## Required GitHub Configuration

### GitHub Secrets

You need to configure the following secret in your GitHub repository under **Settings → Secrets and variables → Actions → Secrets**:

- `AZURE_STATIC_WEB_APPS_API_TOKEN` - The deployment token for your Azure Static Web Apps resource

### Getting the Deployment Token

1. Go to the [Azure Portal](https://portal.azure.com)
2. Navigate to your Azure Static Web Apps resource
3. Go to **Settings → Configuration** or **Overview**
4. Find the **Manage deployment token** button
5. Copy the deployment token
6. Add it as the `AZURE_STATIC_WEB_APPS_API_TOKEN` secret in GitHub

## Workflow Triggers

The workflow is triggered by:
- **Push to `main` branch**: Automatically builds and deploys the Angular application
- **Manual trigger**: Can be triggered manually from the GitHub Actions tab using "Run workflow"

## Workflow Jobs

### Build and Deploy Job

1. **Checkout code**: Checks out the repository code
2. **Setup Node.js**: Installs Node.js 20 with npm caching
3. **Install dependencies**: Runs `npm ci` to install Angular project dependencies
4. **Build Angular application**: Runs `npm run build` to build the production version
5. **Deploy to Azure Static Web Apps**: Deploys the built application using the deployment token

The built application is located at `src/SoftwareConsultingPlatform.WebApp/dist/software-consulting-platform/browser`.

## Infrastructure

The Azure Static Web Apps resource should be created in Azure prior to setting up the CI/CD pipeline. The infrastructure can be provisioned using:
- Azure Portal
- Azure CLI
- Infrastructure as Code tools (Bicep, Terraform, etc.)

The deployment token from the Static Web Apps resource is used for authentication during deployment.

## Troubleshooting

### Workflow Fails with Authentication Error

- Verify that the `AZURE_STATIC_WEB_APPS_API_TOKEN` secret is set correctly in GitHub
- Check that the deployment token hasn't expired or been regenerated
- Ensure you copied the complete token without any extra spaces

### Build Fails

- Check that the Angular project has all required dependencies in package.json
- Verify Node.js version compatibility (workflow uses Node.js 20)
- Review the build logs in the GitHub Actions run
- Test the build locally: `cd src/SoftwareConsultingPlatform.WebApp && npm ci && npm run build`

### Deployment Fails

- Verify the Azure Static Web Apps resource exists
- Check that the deployment token is valid
- Ensure the build output path is correct: `dist/software-consulting-platform/browser`
- Review the deployment logs in the GitHub Actions run

## Local Development and Testing

To test the build locally before pushing to `main`:

1. Navigate to the Angular project:
   ```bash
   cd src/SoftwareConsultingPlatform.WebApp
   ```

2. Install dependencies:
   ```bash
   npm ci
   ```

3. Build the application:
   ```bash
   npm run build
   ```

4. The built application will be in `dist/software-consulting-platform/browser`

## Additional Resources

- [Azure Static Web Apps Documentation](https://learn.microsoft.com/azure/static-web-apps/)
- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Azure Static Web Apps Deploy Action](https://github.com/Azure/static-web-apps-deploy)
