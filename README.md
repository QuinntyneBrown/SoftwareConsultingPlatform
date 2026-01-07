# SoftwareConsultingPlatform

A modern, multi-tenant software consulting platform built with .NET 9 and Angular, featuring comprehensive service management, case studies, and user authentication.

## Overview

SoftwareConsultingPlatform is an enterprise-grade consulting platform that enables software consultants to showcase their services, manage case studies, and deliver content to clients through a responsive web interface. The platform implements a clean architecture pattern with strict separation of concerns and follows Domain-Driven Design principles.

## Features

- **Service Management**: Create, update, and showcase consulting services
- **Case Studies**: Manage and display project case studies with rich content
- **Multi-Tenancy**: Row-level multi-tenancy with TenantId isolation
- **Authentication & Authorization**: JWT-based authentication with tenant-scoped access
- **Homepage Content**: Dynamic homepage content management
- **Responsive UI**: Mobile-first Angular application with Material Design 3
- **Structured Logging**: Comprehensive Serilog integration with Azure Log Analytics

## Architecture

### Backend (.NET 9)

The backend follows a three-layer architecture:

- **SoftwareConsultingPlatform.Core**: Domain models, aggregates, business logic, and service interfaces
- **SoftwareConsultingPlatform.Infrastructure**: Data persistence, Entity Framework, and infrastructure services
- **SoftwareConsultingPlatform.Api**: REST API endpoints, MediatR commands/queries, and DTOs

#### Key Architectural Patterns

- **CQRS with MediatR**: Commands and queries for clean separation of read/write operations
- **Direct DbContext Access**: Uses `ISoftwareConsultingPlatformContext` interface (no repository pattern)
- **Aggregate-Based Domain Model**: Domain entities organized by aggregates (Service, CaseStudy, Homepage, Tenant, User)
- **Extension Method Mapping**: DTOs mapped using `ToDto()` extension methods (no AutoMapper)

### Frontend (Angular)

Modern Angular workspace with Material Design 3:

- **Location**: `src/SoftwareConsultingPlatform.WebApp`
- **State Management**: RxJS-based reactive state (no NgRx, no Signals)
- **UI Components**: Angular Material with async pipe pattern
- **Testing**: Jest for unit tests, Playwright for E2E tests
- **Styling**: BEM naming convention with Material 3 design tokens

## Technology Stack

### Backend
- .NET 9
- Entity Framework Core
- MediatR
- Serilog
- SQL Server Express
- StyleCop.Analyzers

### Frontend
- Angular (latest stable)
- Angular Material
- RxJS
- TypeScript
- Jest
- Playwright

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js and npm
- SQL Server Express
- Azure CLI and Azure Developer CLI (for deployment)

### Local Development

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SoftwareConsultingPlatform
   ```

2. **Backend Setup**
   ```bash
   cd src/SoftwareConsultingPlatform.Api
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

3. **Frontend Setup**
   ```bash
   cd src/SoftwareConsultingPlatform.WebApp
   npm install
   npm start
   ```

4. **Access the application**
   - Frontend: `http://localhost:4200`
   - Backend API: `https://localhost:7200` (or as configured in launchSettings.json)

## CI/CD Pipeline

This repository includes an automated CI/CD pipeline that deploys the Angular application to Azure Static Web Apps on every commit to the `main` branch.

### What's Automated

- **Build**: Automatic build of the Angular application using Node.js and npm
- **Deploy**: Automatic deployment to Azure Static Web Apps using the deployment token

### Setup Requirements

To enable automated deployments, configure the following in your GitHub repository:

**Required Secrets** (Settings → Secrets and variables → Actions → Secrets):
- `AZURE_STATIC_WEB_APPS_API_TOKEN` - Deployment token for the Azure Static Web Apps resource

## Deploy to Azure (azd)

This repo is configured to deploy to Azure using the Azure Developer CLI (`azd`):

### Prerequisites

- Azure CLI (`az`)
- Azure Developer CLI (`azd`)
- Node.js + npm

### Deployment Steps

From the repo root:

1. **Login**
   ```bash
   az login
   azd auth login
   ```

2. **Create environment and deploy**
   ```bash
   azd up
   ```

The `azd` command provisions infrastructure from [infra/main.bicep](infra/main.bicep) and builds the Angular app via `infra/scripts/build-web.ps1`.

## Project Structure

```
SoftwareConsultingPlatform/
├── src/
│   ├── SoftwareConsultingPlatform.Api/          # REST API layer
│   │   ├── Features/                            # MediatR commands & queries
│   │   │   ├── Auth/                            # Authentication features
│   │   │   ├── Homepage/                        # Homepage content features
│   │   │   └── Services/                        # Service management features
│   │   └── Controllers/                         # API controllers
│   ├── SoftwareConsultingPlatform.Core/         # Domain layer
│   │   ├── Models/                              # Domain aggregates
│   │   │   ├── CaseStudyAggregate/
│   │   │   ├── HomepageAggregate/
│   │   │   ├── ServiceAggregate/
│   │   │   ├── TenantAggregate/
│   │   │   └── UserAggregate/
│   │   └── Services/                            # Business logic services
│   ├── SoftwareConsultingPlatform.Infrastructure/ # Infrastructure layer
│   │   ├── Data/                                # EF Core DbContext
│   │   ├── Migrations/                          # EF Core migrations
│   │   └── Configurations/                      # Entity configurations
│   └── SoftwareConsultingPlatform.WebApp/       # Angular frontend
│       └── projects/SoftwareConsultingPlatform/ # Angular project
│           ├── src/
│           │   ├── pages/                       # Routable page components
│           │   ├── components/                  # Reusable components
│           │   └── services/                    # HTTP services
│           └── e2e/                             # Playwright tests
├── tests/                                        # Test projects
├── docs/                                         # Documentation
│   └── specs/                                    # Technical specifications
├── infra/                                        # Azure infrastructure
└── .github/workflows/                            # CI/CD workflows
```

## Development Guidelines

This project follows strict architectural and coding standards defined in [docs/specs/implementation-specs.md](docs/specs/implementation-specs.md).

### Key Conventions

- **No Repository Pattern**: Direct DbContext access via `ISoftwareConsultingPlatformContext`
- **Flat Namespaces**: Namespaces match physical folder structure
- **One Type Per File**: Each class, enum, or record in its own file
- **Async Pipe Pattern**: Frontend uses RxJS observables with async pipe (no manual subscriptions)
- **BEM for CSS**: Block Element Modifier naming for styles
- **Material 3 Only**: Strict adherence to Material Design 3 colors and components

## Testing

### Backend Tests
```bash
dotnet test
```

### Frontend Unit Tests
```bash
cd src/SoftwareConsultingPlatform.WebApp
npm test
```

### E2E Tests
```bash
cd src/SoftwareConsultingPlatform.WebApp
npm run e2e
```

## Contributing

Please ensure all code adheres to the specifications in [docs/specs/implementation-specs.md](docs/specs/implementation-specs.md) before submitting pull requests.

## License

[Specify your license here]