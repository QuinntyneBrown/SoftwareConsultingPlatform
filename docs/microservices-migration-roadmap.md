# Microservices Migration Roadmap

## Event-Driven Architecture with .NET Aspire, YARP API Gateway, and Shared Messaging

**Document Version:** 1.0
**Created:** January 2026
**Target Framework:** .NET 9.0
**Orchestration:** .NET Aspire

---

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Current Architecture Analysis](#2-current-architecture-analysis)
3. [Target Architecture Overview](#3-target-architecture-overview)
4. [Phase 1: Foundation & Infrastructure](#phase-1-foundation--infrastructure)
5. [Phase 2: Shared Projects Setup](#phase-2-shared-projects-setup)
6. [Phase 3: API Gateway Implementation](#phase-3-api-gateway-implementation)
7. [Phase 4: Identity Service Extraction](#phase-4-identity-service-extraction)
8. [Phase 5: Tenant Service Extraction](#phase-5-tenant-service-extraction)
9. [Phase 6: Services Management Microservice](#phase-6-services-management-microservice)
10. [Phase 7: Case Studies Microservice](#phase-7-case-studies-microservice)
11. [Phase 8: Content Management Microservice](#phase-8-content-management-microservice)
12. [Phase 9: Notification Service](#phase-9-notification-service)
13. [Phase 10: Event-Driven Integration](#phase-10-event-driven-integration)
14. [Phase 11: Frontend Migration](#phase-11-frontend-migration)
15. [Phase 12: Testing & Validation](#phase-12-testing--validation)
16. [Phase 13: Deployment & DevOps](#phase-13-deployment--devops)
17. [Appendix A: Message Contracts](#appendix-a-message-contracts)
18. [Appendix B: Project Structure](#appendix-b-project-structure)

---

## 1. Executive Summary

This roadmap outlines the migration from a monolithic ASP.NET Core application to an event-driven microservices architecture using:

- **.NET Aspire** for orchestration, service discovery, and observability
- **YARP (Yet Another Reverse Proxy)** for API Gateway pattern
- **RabbitMQ/Azure Service Bus** for asynchronous messaging
- **Shared project** for message contracts and common code

### Key Benefits
- Independent service deployment and scaling
- Technology flexibility per service
- Improved fault isolation
- Better team autonomy
- Enhanced observability through Aspire Dashboard

---

## 2. Current Architecture Analysis

### 2.1 Existing Structure
```
src/
├── SoftwareConsultingPlatform.Api/        # Monolithic API
│   ├── Controllers/                        # REST endpoints
│   └── Features/                           # CQRS handlers (Auth)
├── SoftwareConsultingPlatform.Core/       # Domain models
├── SoftwareConsultingPlatform.Infrastructure/  # EF Core DbContext
└── Ui/     # Angular frontend
```

### 2.2 Current Bounded Contexts Identified
| Context | Entities | Current Location |
|---------|----------|------------------|
| **Identity** | User, Role, UserRole, RefreshToken, UserSession, ActivityLog | Api/Features/Auth, Core/Aggregates |
| **Tenant Management** | Tenant | Core/Aggregates, Api/Controllers |
| **Services** | Service, ServiceTechnology, ServiceFaq, ServiceInquiry, Technology | Core/Aggregates, Api/Controllers |
| **Case Studies** | CaseStudy, CaseStudyTechnology, CaseStudyImage | Core/Aggregates, Api/Controllers |
| **Content** | HomepageContent, FeaturedItem | Core/Aggregates, Api/Controllers |

### 2.3 Current Pain Points
- Single deployment unit for all features
- Shared database causing coupling
- No async communication
- Scaling requires full application scaling
- All changes require full regression testing

---

## 3. Target Architecture Overview

### 3.1 Target Structure
```
src/
├── Aspire/
│   ├── SoftwareConsultingPlatform.AppHost/           # Aspire orchestrator
│   └── SoftwareConsultingPlatform.ServiceDefaults/   # Shared Aspire configs
├── Shared/
│   ├── Shared.Messages/   # Event/Command contracts
│   ├── Shared.Contracts/  # API DTOs & interfaces
│   └── Shared.Core/       # Common utilities
├── ApiGateway/
│   └── SoftwareConsultingPlatform.ApiGateway/        # YARP gateway
├── Services/
│   ├── Identity/
│   │   ├── SoftwareConsultingPlatform.Identity.Api/
│   │   ├── SoftwareConsultingPlatform.Identity.Core/
│   │   └── SoftwareConsultingPlatform.Identity.Infrastructure/
│   ├── Tenant/
│   │   ├── SoftwareConsultingPlatform.Tenant.Api/
│   │   ├── SoftwareConsultingPlatform.Tenant.Core/
│   │   └── SoftwareConsultingPlatform.Tenant.Infrastructure/
│   ├── Services/
│   │   ├── SoftwareConsultingPlatform.Services.Api/
│   │   ├── SoftwareConsultingPlatform.Services.Core/
│   │   └── SoftwareConsultingPlatform.Services.Infrastructure/
│   ├── CaseStudies/
│   │   ├── SoftwareConsultingPlatform.CaseStudies.Api/
│   │   ├── SoftwareConsultingPlatform.CaseStudies.Core/
│   │   └── SoftwareConsultingPlatform.CaseStudies.Infrastructure/
│   ├── Content/
│   │   ├── SoftwareConsultingPlatform.Content.Api/
│   │   ├── SoftwareConsultingPlatform.Content.Core/
│   │   └── SoftwareConsultingPlatform.Content.Infrastructure/
│   └── Notification/
│       └── SoftwareConsultingPlatform.Notification.Api/
└── WebApp/
    └── Ui/            # Angular frontend
```

### 3.2 Architecture Diagram
```
                                    ┌─────────────────────────────────────┐
                                    │         .NET Aspire AppHost         │
                                    │   (Orchestration & Service Discovery)│
                                    └─────────────────────────────────────┘
                                                      │
                    ┌─────────────────────────────────┼─────────────────────────────────┐
                    │                                 │                                 │
              ┌─────▼─────┐                    ┌──────▼──────┐                   ┌──────▼──────┐
              │  Angular  │                    │   Message   │                   │   Aspire    │
              │  WebApp   │                    │    Broker   │                   │  Dashboard  │
              └─────┬─────┘                    │ (RabbitMQ)  │                   │(Observability)│
                    │                          └──────┬──────┘                   └─────────────┘
                    │                                 │
              ┌─────▼─────────────────────────────────┼────────────────────────────────────┐
              │                            YARP API Gateway                                 │
              │                    (Routing, Auth, Rate Limiting)                          │
              └─────┬─────────────┬─────────────┬─────────────┬─────────────┬─────────────┘
                    │             │             │             │             │
              ┌─────▼────┐  ┌─────▼────┐  ┌─────▼────┐  ┌─────▼────┐  ┌─────▼────┐
              │ Identity │  │  Tenant  │  │ Services │  │  Case    │  │ Content  │
              │ Service  │  │ Service  │  │ Service  │  │ Studies  │  │ Service  │
              │          │  │          │  │          │  │ Service  │  │          │
              └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘
                   │             │             │             │             │
              ┌────▼────┐   ┌────▼────┐   ┌────▼────┐   ┌────▼────┐   ┌────▼────┐
              │Identity │   │ Tenant  │   │Services │   │  Case   │   │ Content │
              │   DB    │   │   DB    │   │   DB    │   │Study DB │   │   DB    │
              └─────────┘   └─────────┘   └─────────┘   └─────────┘   └─────────┘
```

---

## Phase 1: Foundation & Infrastructure

### 1.1 Create Aspire Solution Structure

#### Item 1.1.1: Create New Solution File
- [x] Create `SoftwareConsultingPlatform.Microservices.sln` in repository root
- [x] Keep existing solution for reference during migration

#### Item 1.1.2: Create AppHost Project
- [x] Create `src/SoftwareConsultingPlatform.AppHost/`
- [x] Add Aspire.Hosting package references
- [x] Configure Program.cs with distributed application builder

```csharp
// src/SoftwareConsultingPlatform.AppHost/Program.cs
var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var rabbitMq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();

var redis = builder.AddRedis("cache");

// Databases (per service)
var identityDb = builder.AddSqlServer("sql")
    .AddDatabase("identity-db");
var tenantDb = builder.AddSqlServer("sql")
    .AddDatabase("tenant-db");
var servicesDb = builder.AddSqlServer("sql")
    .AddDatabase("services-db");
var caseStudiesDb = builder.AddSqlServer("sql")
    .AddDatabase("casestudies-db");
var contentDb = builder.AddSqlServer("sql")
    .AddDatabase("content-db");

// Services will be added in subsequent phases...

builder.Build().Run();
```

#### Item 1.1.3: Create ServiceDefaults Project
- [x] Create `src/SoftwareConsultingPlatform.ServiceDefaults/`
- [x] Add shared service configuration extensions
- [x] Configure OpenTelemetry, health checks, resilience

```csharp
// src/SoftwareConsultingPlatform.ServiceDefaults/Extensions.cs
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });
        return builder;
    }
}
```

#### Item 1.1.4: Configure Development Certificates
- [x] Set up HTTPS development certificates for all services
- [x] Configure certificate sharing via Aspire

### 1.2 Message Broker Setup

#### Item 1.2.1: Add RabbitMQ to AppHost
- [x] Configure RabbitMQ container in AppHost
- [x] Enable management plugin for monitoring
- [x] Configure persistence volumes

#### Item 1.2.2: Create Message Broker Abstraction
- [x] Define `IMessagePublisher` interface
- [x] Define `IMessageConsumer` interface
- [x] Implement MassTransit integration

### 1.3 Database Migration Strategy

#### Item 1.3.1: Document Data Dependencies
- [x] Map cross-service data relationships
- [x] Identify shared lookup data (Technologies)
- [x] Plan data synchronization strategy

#### Item 1.3.2: Create Database Migration Scripts
- [x] Script to split monolithic database
- [x] Script to create service-specific databases
- [x] Data migration validation queries

---

## Phase 2: Shared Projects Setup

### 2.1 Create Shared.Messages Project

#### Item 2.1.1: Project Setup
- [x] Create `src/Shared/Shared.Messages/`
- [x] Add MassTransit abstractions package
- [x] Configure project for multi-targeting if needed

#### Item 2.1.2: Define Integration Events

```csharp
// src/Shared/Shared.Messages/Events/Identity/
namespace Shared.Messages.Events.Identity;

public record UserRegisteredEvent(
    Guid UserId,
    Guid TenantId,
    string Email,
    string FullName,
    DateTime RegisteredAt
);

public record UserEmailVerifiedEvent(
    Guid UserId,
    Guid TenantId,
    DateTime VerifiedAt
);

public record UserLoggedInEvent(
    Guid UserId,
    Guid TenantId,
    string IpAddress,
    string DeviceInfo,
    DateTime LoggedInAt
);

public record UserLockedEvent(
    Guid UserId,
    Guid TenantId,
    DateTime LockedUntil,
    string Reason
);

public record PasswordResetRequestedEvent(
    Guid UserId,
    Guid TenantId,
    string Email,
    string ResetToken,
    DateTime ExpiresAt
);
```

#### Item 2.1.3: Define Tenant Events
```csharp
// src/Shared/Shared.Messages/Events/Tenant/
namespace Shared.Messages.Events.Tenant;

public record TenantCreatedEvent(
    Guid TenantId,
    string Name,
    string Subdomain,
    DateTime CreatedAt
);

public record TenantActivatedEvent(
    Guid TenantId,
    DateTime ActivatedAt
);

public record TenantSuspendedEvent(
    Guid TenantId,
    string Reason,
    DateTime SuspendedAt
);

public record TenantBrandingUpdatedEvent(
    Guid TenantId,
    string? LogoUrl,
    string? PrimaryColor,
    string? SecondaryColor
);
```

#### Item 2.1.4: Define Services Events
```csharp
// src/Shared/Shared.Messages/Events/Services/
namespace Shared.Messages.Events.Services;

public record ServicePublishedEvent(
    Guid ServiceId,
    Guid TenantId,
    string Name,
    string Slug,
    DateTime PublishedAt
);

public record ServiceArchivedEvent(
    Guid ServiceId,
    Guid TenantId,
    DateTime ArchivedAt
);

public record ServiceInquirySubmittedEvent(
    Guid InquiryId,
    Guid ServiceId,
    Guid TenantId,
    string Name,
    string Email,
    string? Company,
    string ProjectDescription,
    DateTime SubmittedAt
);

public record ServiceFeaturedChangedEvent(
    Guid ServiceId,
    Guid TenantId,
    bool IsFeatured,
    int? DisplayOrder
);
```

#### Item 2.1.5: Define Case Studies Events
```csharp
// src/Shared/Shared.Messages/Events/CaseStudies/
namespace Shared.Messages.Events.CaseStudies;

public record CaseStudyPublishedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    string ClientName,
    string ProjectTitle,
    string Slug,
    DateTime PublishedAt
);

public record CaseStudyArchivedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    DateTime ArchivedAt
);

public record CaseStudyFeaturedChangedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    bool IsFeatured,
    int? FeaturedOrder
);
```

#### Item 2.1.6: Define Content Events
```csharp
// src/Shared/Shared.Messages/Events/Content/
namespace Shared.Messages.Events.Content;

public record HomepageContentUpdatedEvent(
    Guid ContentId,
    Guid TenantId,
    string UpdatedBy,
    DateTime UpdatedAt
);

public record FeaturedItemAddedEvent(
    Guid TenantId,
    string ItemType,
    Guid ItemId,
    int DisplayOrder
);

public record FeaturedItemRemovedEvent(
    Guid TenantId,
    string ItemType,
    Guid ItemId
);
```

#### Item 2.1.7: Define Notification Commands
```csharp
// src/Shared/Shared.Messages/Commands/
namespace Shared.Messages.Commands;

public record SendEmailCommand(
    Guid TenantId,
    string To,
    string Subject,
    string TemplateName,
    Dictionary<string, string> TemplateData
);

public record SendWelcomeEmailCommand(
    Guid TenantId,
    string Email,
    string FullName,
    string VerificationUrl
);

public record SendPasswordResetEmailCommand(
    Guid TenantId,
    string Email,
    string ResetUrl,
    DateTime ExpiresAt
);

public record SendInquiryNotificationCommand(
    Guid TenantId,
    Guid ServiceId,
    string ServiceName,
    string InquirerName,
    string InquirerEmail,
    string ProjectDescription
);
```

### 2.2 Create Shared.Contracts Project

#### Item 2.2.1: Project Setup
- [x] Create `src/Shared/Shared.Contracts/`
- [x] Define API response DTOs
- [x] Define shared interfaces

#### Item 2.2.2: Define API Contracts
```csharp
// src/Shared/Shared.Contracts/Identity/
namespace Shared.Contracts.Identity;

public record UserDto(
    Guid UserId,
    string Email,
    string FullName,
    string? Phone,
    string? CompanyName,
    string? AvatarUrl,
    bool EmailVerified,
    bool MfaEnabled,
    string Status
);

public record AuthTokenDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
```

#### Item 2.2.3: Define Service Contracts
```csharp
// src/Shared/Shared.Contracts/Services/
namespace Shared.Contracts.Services;

public record ServiceSummaryDto(
    Guid ServiceId,
    string Name,
    string Slug,
    string? Tagline,
    string? IconUrl,
    bool Featured,
    int DisplayOrder
);

public record ServiceDetailDto(
    Guid ServiceId,
    string Name,
    string Slug,
    string? Tagline,
    string? Overview,
    string? IconUrl,
    List<string> WhatWeDo,
    List<string> HowWeWork,
    List<string> Benefits,
    List<PricingModelDto> PricingModels,
    List<EngagementTypeDto> EngagementTypes,
    List<TechnologyDto> Technologies,
    List<FaqDto> Faqs
);
```

### 2.3 Create Shared.Core Project

#### Item 2.3.1: Project Setup
- [x] Create `src/Shared/Shared.Core/`
- [x] Add common utilities and extensions

#### Item 2.3.2: Define Common Types
```csharp
// src/Shared/Shared.Core/
namespace Shared.Core;

public interface ITenantContext
{
    Guid TenantId { get; }
}

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string Email { get; }
    IEnumerable<string> Roles { get; }
}

public abstract record DomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
```

#### Item 2.3.3: Add Common Extensions
```csharp
// src/Shared/Shared.Core/Extensions/
namespace Shared.Core.Extensions;

public static class StringExtensions
{
    public static string ToSlug(this string value) { /* ... */ }
    public static string Truncate(this string value, int maxLength) { /* ... */ }
}

public static class GuidExtensions
{
    public static bool IsEmpty(this Guid value) => value == Guid.Empty;
}
```

---

## Phase 3: API Gateway Implementation

### 3.1 Create YARP Gateway Project

#### Item 3.1.1: Project Setup
- [x] Create `src/ApiGateway/SoftwareConsultingPlatform.ApiGateway/`
- [x] Add YARP package (Yarp.ReverseProxy)
- [x] Add ServiceDefaults reference

#### Item 3.1.2: Configure YARP Routing
```csharp
// src/ApiGateway/SoftwareConsultingPlatform.ApiGateway/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* JWT config */ });

builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddRateLimiter(options => { /* rate limiting */ });

var app = builder.Build();

app.UseServiceDefaults();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();
```

#### Item 3.1.3: Configure Route Mappings
```json
// src/ApiGateway/SoftwareConsultingPlatform.ApiGateway/appsettings.json
{
  "ReverseProxy": {
    "Routes": {
      "identity-route": {
        "ClusterId": "identity-cluster",
        "Match": {
          "Path": "/api/auth/{**catch-all}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/api" }
        ]
      },
      "users-route": {
        "ClusterId": "identity-cluster",
        "Match": {
          "Path": "/api/users/{**catch-all}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/api" }
        ],
        "AuthorizationPolicy": "authenticated"
      },
      "tenants-route": {
        "ClusterId": "tenant-cluster",
        "Match": {
          "Path": "/api/tenants/{**catch-all}"
        }
      },
      "services-route": {
        "ClusterId": "services-cluster",
        "Match": {
          "Path": "/api/services/{**catch-all}"
        }
      },
      "case-studies-route": {
        "ClusterId": "casestudies-cluster",
        "Match": {
          "Path": "/api/case-studies/{**catch-all}"
        }
      },
      "homepage-route": {
        "ClusterId": "content-cluster",
        "Match": {
          "Path": "/api/homepage/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "identity-cluster": {
        "Destinations": {
          "identity-service": {
            "Address": "https+http://identity-service"
          }
        }
      },
      "tenant-cluster": {
        "Destinations": {
          "tenant-service": {
            "Address": "https+http://tenant-service"
          }
        }
      },
      "services-cluster": {
        "Destinations": {
          "services-service": {
            "Address": "https+http://services-service"
          }
        }
      },
      "casestudies-cluster": {
        "Destinations": {
          "casestudies-service": {
            "Address": "https+http://casestudies-service"
          }
        }
      },
      "content-cluster": {
        "Destinations": {
          "content-service": {
            "Address": "https+http://content-service"
          }
        }
      }
    }
  }
}
```

### 3.2 Gateway Features

#### Item 3.2.1: Implement Rate Limiting
- [x] Configure rate limiting policies per route
- [x] Add IP-based rate limiting
- [x] Add user-based rate limiting for authenticated routes

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 10;
    });

    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 6;
        opt.PermitLimit = 60;
    });
});
```

#### Item 3.2.2: Implement Request/Response Transformation
- [x] Add tenant header injection
- [x] Add correlation ID propagation
- [x] Add response caching headers

#### Item 3.2.3: Implement Health Check Aggregation
- [x] Aggregate health from all downstream services
- [x] Expose `/health` endpoint with detailed status

#### Item 3.2.4: Add OpenAPI Aggregation
- [x] Aggregate Swagger docs from all services
- [x] Expose unified `/swagger` endpoint

### 3.3 Register Gateway in AppHost

#### Item 3.3.1: Update AppHost Configuration
```csharp
// In Program.cs
var apiGateway = builder.AddProject<Projects.SoftwareConsultingPlatform_ApiGateway>("api-gateway")
    .WithReference(identityService)
    .WithReference(tenantService)
    .WithReference(servicesService)
    .WithReference(caseStudiesService)
    .WithReference(contentService)
    .WithExternalHttpEndpoints();
```

---

## Phase 4: Identity Service Extraction

### 4.1 Create Identity Service Projects

#### Item 4.1.1: Create Identity.Api Project
- [x] Create `src/Services/Identity/SoftwareConsultingPlatform.Identity.Api/`
- [x] Add ServiceDefaults reference
- [x] Add Shared.Messages reference
- [x] Add Shared.Contracts reference

#### Item 4.1.2: Create Identity.Core Project
- [x] Create `src/Services/Identity/SoftwareConsultingPlatform.Identity.Core/`
- [x] Move User, Role, UserRole, RefreshToken, UserSession, ActivityLog entities
- [x] Define Identity-specific domain events

#### Item 4.1.3: Create Identity.Infrastructure Project
- [x] Create `src/Services/Identity/SoftwareConsultingPlatform.Identity.Infrastructure/`
- [x] Create IdentityDbContext
- [x] Configure entity mappings

### 4.2 Migrate Authentication Features

#### Item 4.2.1: Migrate Auth Controllers
- [x] Move `AuthController` from monolith
- [x] Move `UsersController` from monolith
- [x] Update to use Identity-specific context

#### Item 4.2.2: Migrate Auth Handlers
- [x] Move `RegisterCommandHandler`
- [x] Move `LoginCommandHandler`
- [x] Move all auth-related MediatR handlers

#### Item 4.2.3: Implement Event Publishing
```csharp
// After user registration
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        // ... create user logic ...

        await _publishEndpoint.Publish(new UserRegisteredEvent(
            user.UserId.Value,
            user.TenantId.Value,
            user.Email,
            user.FullName,
            DateTime.UtcNow
        ), ct);

        await _publishEndpoint.Publish(new SendWelcomeEmailCommand(
            user.TenantId.Value,
            user.Email,
            user.FullName,
            verificationUrl
        ), ct);

        return response;
    }
}
```

### 4.3 Configure Identity Service

#### Item 4.3.1: Configure Program.cs
```csharp
// src/Services/Identity/SoftwareConsultingPlatform.Identity.Api/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<IdentityDbContext>("identity-db");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();
app.UseServiceDefaults();
app.MapControllers();
app.Run();
```

#### Item 4.3.2: Register in AppHost
```csharp
var identityService = builder.AddProject<Projects.SoftwareConsultingPlatform_Identity_Api>("identity-service")
    .WithReference(identityDb)
    .WithReference(rabbitMq)
    .WithReference(redis);
```

### 4.4 Create Database Migration

#### Item 4.4.1: Create IdentityDbContext
- [x] Define DbSets for identity entities only
- [x] Remove tenant filtering (handled at API level)
- [x] Add migration for separate identity database

#### Item 4.4.2: Data Migration Script
- [x] Create script to copy users to identity database
- [x] Create script to copy roles and mappings
- [x] Verify data integrity post-migration

---

## Phase 5: Tenant Service Extraction

### 5.1 Create Tenant Service Projects

#### Item 5.1.1: Create Tenant.Api Project
- [x] Create `src/Services/Tenant/SoftwareConsultingPlatform.Tenant.Api/`
- [x] Add necessary package references

#### Item 5.1.2: Create Tenant.Core Project
- [x] Create `src/Services/Tenant/SoftwareConsultingPlatform.Tenant.Core/`
- [x] Move Tenant entity and value objects
- [x] Define tenant domain events

#### Item 5.1.3: Create Tenant.Infrastructure Project
- [x] Create `src/Services/Tenant/SoftwareConsultingPlatform.Tenant.Infrastructure/`
- [x] Create TenantDbContext

### 5.2 Implement Tenant API

#### Item 5.2.1: Create Tenant Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetAll() { }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantDto>> GetById(Guid id) { }

    [HttpGet("by-subdomain/{subdomain}")]
    public async Task<ActionResult<TenantDto>> GetBySubdomain(string subdomain) { }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<TenantDto>> Create(CreateTenantRequest request) { }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<TenantDto>> Update(Guid id, UpdateTenantRequest request) { }
}
```

#### Item 5.2.2: Implement Event Publishing
- [x] Publish `TenantCreatedEvent` on creation
- [x] Publish `TenantActivatedEvent` on activation
- [x] Publish `TenantBrandingUpdatedEvent` on branding changes

### 5.3 Cross-Service Tenant Resolution

#### Item 5.3.1: Create Tenant Resolution Middleware
- [x] Resolve tenant from subdomain
- [x] Resolve tenant from custom domain
- [x] Resolve tenant from header (for internal services)

#### Item 5.3.2: Cache Tenant Data
- [x] Implement Redis caching for tenant lookup
- [x] Subscribe to tenant update events to invalidate cache

### 5.4 Register Tenant Service in AppHost

#### Item 5.4.1: Update AppHost
```csharp
var tenantService = builder.AddProject<Projects.SoftwareConsultingPlatform_Tenant_Api>("tenant-service")
    .WithReference(tenantDb)
    .WithReference(rabbitMq)
    .WithReference(redis);
```

---

## Phase 6: Services Management Microservice

### 6.1 Create Services Projects

#### Item 6.1.1: Create Services.Api Project
- [x] Create `src/Services/Services/SoftwareConsultingPlatform.Services.Api/`
- [x] Configure Aspire service defaults

#### Item 6.1.2: Create Services.Core Project
- [x] Create `src/Services/Services/SoftwareConsultingPlatform.Services.Core/`
- [x] Move Service, ServiceTechnology, ServiceFaq, ServiceInquiry entities
- [x] Move Technology entity (or reference from shared)

#### Item 6.1.3: Create Services.Infrastructure Project
- [x] Create `src/Services/Services/SoftwareConsultingPlatform.Services.Infrastructure/`
- [x] Create ServicesDbContext

### 6.2 Implement Services API

#### Item 6.2.1: Migrate ServicesController
- [x] Move from monolith with modifications
- [x] Update to use service-specific DbContext
- [x] Add event publishing

#### Item 6.2.2: Implement CQRS Handlers
```csharp
public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceDto>
{
    public async Task<ServiceDto> Handle(CreateServiceCommand request, CancellationToken ct)
    {
        var service = new Service(/* ... */);
        _context.Services.Add(service);
        await _context.SaveChangesAsync(ct);

        // Publish event for other services
        await _publishEndpoint.Publish(new ServiceCreatedEvent(
            service.ServiceId.Value,
            service.TenantId.Value,
            service.Name,
            service.Slug
        ), ct);

        return service.ToDto();
    }
}
```

#### Item 6.2.3: Handle Service Inquiries
- [x] Create inquiry submission endpoint
- [x] Publish `ServiceInquirySubmittedEvent`
- [x] Trigger notification via command

### 6.3 Event Consumers

#### Item 6.3.1: Consume Tenant Events
```csharp
public class TenantSuspendedEventConsumer : IConsumer<TenantSuspendedEvent>
{
    public async Task Consume(ConsumeContext<TenantSuspendedEvent> context)
    {
        // Archive all services for suspended tenant
        var services = await _context.Services
            .Where(s => s.TenantId == context.Message.TenantId)
            .ToListAsync();

        foreach (var service in services)
        {
            service.Archive();
        }

        await _context.SaveChangesAsync();
    }
}
```

### 6.4 Register Services Service in AppHost

#### Item 6.4.1: Update AppHost
```csharp
var servicesService = builder.AddProject<Projects.SoftwareConsultingPlatform_Services_Api>("services-service")
    .WithReference(servicesDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(tenantService);
```

---

## Phase 7: Case Studies Microservice

### 7.1 Create Case Studies Projects

#### Item 7.1.1: Create CaseStudies.Api Project
- [x] Create `src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Api/`
- [x] Configure Aspire service defaults

#### Item 7.1.2: Create CaseStudies.Core Project
- [x] Create `src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Core/`
- [x] Move CaseStudy, CaseStudyTechnology, CaseStudyImage entities

#### Item 7.1.3: Create CaseStudies.Infrastructure Project
- [x] Create `src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Infrastructure/`
- [x] Create CaseStudiesDbContext

### 7.2 Implement Full CRUD API

#### Item 7.2.1: Complete CaseStudiesController
- [x] Implement `GET /api/case-studies` (currently returns 501)
- [x] Implement `GET /api/case-studies/{id}`
- [x] Implement `GET /api/case-studies/by-slug/{slug}`
- [x] Implement `GET /api/case-studies/search`
- [x] Implement `POST /api/case-studies`
- [x] Implement `PUT /api/case-studies/{id}`
- [x] Implement `DELETE /api/case-studies/{id}`

#### Item 7.2.2: Implement Featured Case Studies
- [x] Migrate featured case studies query
- [x] Add event publishing for featured changes

### 7.3 Event Publishing & Consuming

#### Item 7.3.1: Publish Case Study Events
- [x] `CaseStudyPublishedEvent` on publish
- [x] `CaseStudyArchivedEvent` on archive
- [x] `CaseStudyFeaturedChangedEvent` on featured toggle

#### Item 7.3.2: Consume Tenant Events
- [x] Handle `TenantSuspendedEvent` - archive case studies

### 7.4 Register Case Studies Service in AppHost

#### Item 7.4.1: Update AppHost
```csharp
var caseStudiesService = builder.AddProject<Projects.SoftwareConsultingPlatform_CaseStudies_Api>("casestudies-service")
    .WithReference(caseStudiesDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(tenantService);
```

---

## Phase 8: Content Management Microservice

### 8.1 Create Content Projects

#### Item 8.1.1: Create Content.Api Project
- [x] Create `src/Services/Content/SoftwareConsultingPlatform.Content.Api/`
- [x] Configure Aspire service defaults

#### Item 8.1.2: Create Content.Core Project
- [x] Create `src/Services/Content/SoftwareConsultingPlatform.Content.Core/`
- [x] Move HomepageContent, FeaturedItem entities

#### Item 8.1.3: Create Content.Infrastructure Project
- [x] Create `src/Services/Content/SoftwareConsultingPlatform.Content.Infrastructure/`
- [x] Create ContentDbContext

### 8.2 Implement Content API

#### Item 8.2.1: Homepage Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class HomepageController : ControllerBase
{
    [HttpGet("content")]
    public async Task<ActionResult<HomepageContentDto>> GetContent() { }

    [HttpPut("content")]
    [Authorize]
    public async Task<ActionResult> UpdateContent(UpdateHomepageContentRequest request) { }

    [HttpGet("featured-services")]
    public async Task<ActionResult<IEnumerable<ServiceSummaryDto>>> GetFeaturedServices() { }

    [HttpGet("featured-case-studies")]
    public async Task<ActionResult<IEnumerable<CaseStudySummaryDto>>> GetFeaturedCaseStudies() { }
}
```

### 8.3 Cross-Service Data Aggregation

#### Item 8.3.1: Implement Service Client
- [x] Create `IServicesServiceClient` for fetching service data
- [x] Use HTTP client with service discovery

```csharp
public class ServicesServiceClient : IServicesServiceClient
{
    private readonly HttpClient _httpClient;

    public ServicesServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ServiceSummaryDto>> GetFeaturedServicesAsync(Guid tenantId)
    {
        var response = await _httpClient.GetAsync($"/services/featured?tenantId={tenantId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ServiceSummaryDto>>();
    }
}
```

#### Item 8.3.2: Implement Case Studies Client
- [x] Create `ICaseStudiesServiceClient` for fetching case study data
- [x] Handle failures gracefully with fallbacks

### 8.4 Event Consumers for Cache Invalidation

#### Item 8.4.1: Consume Service Events
```csharp
public class ServiceFeaturedChangedEventConsumer : IConsumer<ServiceFeaturedChangedEvent>
{
    private readonly IDistributedCache _cache;

    public async Task Consume(ConsumeContext<ServiceFeaturedChangedEvent> context)
    {
        // Invalidate featured services cache
        await _cache.RemoveAsync($"featured-services:{context.Message.TenantId}");
    }
}
```

#### Item 8.4.2: Consume Case Study Events
- [x] Invalidate cache on `CaseStudyFeaturedChangedEvent`
- [x] Update local references on `CaseStudyPublishedEvent`

### 8.5 Register Content Service in AppHost

#### Item 8.5.1: Update AppHost
```csharp
var contentService = builder.AddProject<Projects.SoftwareConsultingPlatform_Content_Api>("content-service")
    .WithReference(contentDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(servicesService)
    .WithReference(caseStudiesService)
    .WithReference(tenantService);
```

---

## Phase 9: Notification Service

### 9.1 Create Notification Service

#### Item 9.1.1: Create Notification.Api Project
- [x] Create `src/Services/Notification/SoftwareConsultingPlatform.Notification.Api/`
- [x] Add email provider packages (SendGrid, SMTP, etc.)

### 9.2 Implement Email Functionality

#### Item 9.2.1: Email Service Interface
```csharp
public interface IEmailService
{
    Task SendAsync(EmailMessage message, CancellationToken ct = default);
    Task SendTemplatedAsync(string templateName, string to, Dictionary<string, string> data, CancellationToken ct = default);
}
```

#### Item 9.2.2: Email Templates
- [x] Welcome email template
- [x] Password reset email template
- [x] Email verification template
- [x] Service inquiry notification template

### 9.3 Event Consumers

#### Item 9.3.1: Implement Send Email Command Consumer
```csharp
public class SendEmailCommandConsumer : IConsumer<SendEmailCommand>
{
    private readonly IEmailService _emailService;

    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        var message = context.Message;
        await _emailService.SendTemplatedAsync(
            message.TemplateName,
            message.To,
            message.TemplateData
        );
    }
}
```

#### Item 9.3.2: Welcome Email Consumer
```csharp
public class SendWelcomeEmailCommandConsumer : IConsumer<SendWelcomeEmailCommand>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmailCommand> context)
    {
        var msg = context.Message;
        await _emailService.SendTemplatedAsync("welcome", msg.Email, new Dictionary<string, string>
        {
            ["FullName"] = msg.FullName,
            ["VerificationUrl"] = msg.VerificationUrl
        });
    }
}
```

#### Item 9.3.3: Inquiry Notification Consumer
```csharp
public class SendInquiryNotificationCommandConsumer : IConsumer<SendInquiryNotificationCommand>
{
    public async Task Consume(ConsumeContext<SendInquiryNotificationCommand> context)
    {
        // Send notification to tenant admin about new inquiry
    }
}
```

### 9.4 Register Notification Service in AppHost

#### Item 9.4.1: Update AppHost
```csharp
var notificationService = builder.AddProject<Projects.SoftwareConsultingPlatform_Notification_Api>("notification-service")
    .WithReference(rabbitMq);
```

---

## Phase 10: Event-Driven Integration

### 10.1 Configure MassTransit Across Services

#### Item 10.1.1: Create Shared MassTransit Configuration
```csharp
// In Shared.Core or ServiceDefaults
public static class MassTransitExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator> configure)
    {
        services.AddMassTransit(x =>
        {
            configure(x);

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("messaging"));

                cfg.UseMessageRetry(r => r.Exponential(5,
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromSeconds(5)));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
```

### 10.2 Implement Saga/Choreography Patterns

#### Item 10.2.1: User Registration Saga
```
UserRegisteredEvent
  → Identity Service publishes
  → Notification Service consumes → Sends welcome email
  → Content Service consumes → Creates default homepage content
```

#### Item 10.2.2: Service Inquiry Saga
```
ServiceInquirySubmittedEvent
  → Services Service publishes
  → Notification Service consumes → Sends admin notification
  → Notification Service consumes → Sends confirmation to inquirer
```

### 10.3 Implement Outbox Pattern

#### Item 10.3.1: Add Outbox Support
- [x] Add MassTransit.EntityFrameworkCore package
- [x] Configure outbox in each service's DbContext
- [x] Ensure transactional consistency

```csharp
services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<IdentityDbContext>(o =>
    {
        o.UseSqlServer();
        o.UseBusOutbox();
    });
});
```

### 10.4 Event Versioning Strategy

#### Item 10.4.1: Define Versioning Approach
- [x] Use message headers for version info
- [x] Implement backward-compatible changes
- [x] Document breaking change process

---

## Phase 11: Frontend Migration

### 11.1 Update API Client Configuration

#### Item 11.1.1: Update Environment Configuration
```typescript
// src/environments/environment.ts
export const environment = {
  production: false,
  apiGatewayUrl: 'http://localhost:5000/api' // YARP Gateway
};
```

#### Item 11.1.2: Update HTTP Interceptors
- [x] Update base URL to point to API Gateway
- [x] Ensure tenant header propagation
- [x] Update error handling for microservices responses

### 11.2 Update Service Clients

#### Item 11.2.1: Update AuthService
- [x] Verify endpoints match gateway routing
- [x] Test authentication flow end-to-end

#### Item 11.2.2: Update Other Services
- [x] Update ServicesService
- [x] Update CaseStudiesService
- [x] Update LandingService

### 11.3 Add Resilience

#### Item 11.3.1: Implement Retry Logic
```typescript
// Add retry interceptor for transient failures
@Injectable()
export class RetryInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      retry({
        count: 3,
        delay: (error, retryCount) => {
          if (error.status >= 500) {
            return timer(1000 * retryCount);
          }
          throw error;
        }
      })
    );
  }
}
```

---

## Phase 12: Testing & Validation

### 12.1 Unit Testing

#### Item 12.1.1: Service-Level Unit Tests
- [x] Test each microservice's handlers independently
- [x] Mock message publishing
- [x] Test domain logic

#### Item 12.1.2: Event Contract Tests
- [x] Verify event serialization/deserialization
- [x] Test backward compatibility

### 12.2 Integration Testing

#### Item 12.2.1: Test with Aspire Test Host
```csharp
public class IntegrationTests
{
    [Fact]
    public async Task UserRegistration_PublishesEvent_And_SendsEmail()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.SoftwareConsultingPlatform_AppHost>();

        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var httpClient = app.CreateHttpClient("api-gateway");

        // Test registration flow
        var response = await httpClient.PostAsJsonAsync("/api/auth/register", new
        {
            Email = "test@example.com",
            Password = "Test123!",
            FullName = "Test User"
        });

        response.EnsureSuccessStatusCode();

        // Verify event was published and processed
        // ...
    }
}
```

#### Item 12.2.2: Cross-Service Integration Tests
- [x] Test complete user registration flow
- [x] Test service inquiry flow
- [x] Test content aggregation

### 12.3 End-to-End Testing

#### Item 12.3.1: E2E Test Suite
- [x] Test through API Gateway
- [x] Test frontend to backend flows
- [x] Test failure scenarios and recovery

### 12.4 Performance Testing

#### Item 12.4.1: Load Testing
- [x] Test individual service performance
- [x] Test gateway throughput
- [x] Test message broker under load

---

## Phase 13: Deployment & DevOps

### 13.1 Container Configuration

#### Item 13.1.1: Create Dockerfiles
```dockerfile
# Example: Identity Service Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Services/Identity/SoftwareConsultingPlatform.Identity.Api/", "Services/Identity/SoftwareConsultingPlatform.Identity.Api/"]
# ... copy other projects
RUN dotnet restore "Services/Identity/SoftwareConsultingPlatform.Identity.Api/SoftwareConsultingPlatform.Identity.Api.csproj"
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareConsultingPlatform.Identity.Api.dll"]
```

### 13.2 Kubernetes/Container Orchestration

#### Item 13.2.1: Generate Aspire Manifests
```bash
dotnet run --project src/SoftwareConsultingPlatform.AppHost -- --publisher manifest --output-path ./manifests
```

#### Item 13.2.2: Create Kubernetes Manifests
- [x] Generate from Aspire or create manually
- [x] Configure service meshes if needed
- [x] Set up ingress for API Gateway

### 13.3 CI/CD Pipeline

#### Item 13.3.1: Build Pipeline
```yaml
# .github/workflows/build.yml
name: Build and Test

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build
```

#### Item 13.3.2: Deployment Pipeline
- [x] Configure per-service deployments
- [x] Set up database migrations
- [x] Configure blue-green or canary deployments

### 13.4 Monitoring & Observability

#### Item 13.4.1: Configure Aspire Dashboard
- [x] Enable distributed tracing
- [x] Configure log aggregation
- [x] Set up metrics collection

#### Item 13.4.2: Production Monitoring
- [x] Configure alerts for service health
- [x] Set up message queue monitoring
- [x] Configure database monitoring

---

## Appendix A: Message Contracts

### Complete Event Catalog

| Event | Publisher | Consumers | Purpose |
|-------|-----------|-----------|---------|
| `UserRegisteredEvent` | Identity | Notification, Content | New user created |
| `UserEmailVerifiedEvent` | Identity | - | Email verified |
| `UserLoggedInEvent` | Identity | - | Audit logging |
| `UserLockedEvent` | Identity | Notification | Account locked |
| `PasswordResetRequestedEvent` | Identity | Notification | Reset requested |
| `TenantCreatedEvent` | Tenant | Identity, Content | New tenant |
| `TenantActivatedEvent` | Tenant | All Services | Tenant active |
| `TenantSuspendedEvent` | Tenant | All Services | Tenant suspended |
| `TenantBrandingUpdatedEvent` | Tenant | Content | Branding changed |
| `ServicePublishedEvent` | Services | Content | Service published |
| `ServiceArchivedEvent` | Services | Content | Service archived |
| `ServiceInquirySubmittedEvent` | Services | Notification | New inquiry |
| `ServiceFeaturedChangedEvent` | Services | Content | Featured status changed |
| `CaseStudyPublishedEvent` | CaseStudies | Content | Case study published |
| `CaseStudyArchivedEvent` | CaseStudies | Content | Case study archived |
| `CaseStudyFeaturedChangedEvent` | CaseStudies | Content | Featured status changed |
| `HomepageContentUpdatedEvent` | Content | - | Content updated |

### Command Catalog

| Command | Publisher | Consumer | Purpose |
|---------|-----------|----------|---------|
| `SendEmailCommand` | Any | Notification | Generic email |
| `SendWelcomeEmailCommand` | Identity | Notification | Welcome email |
| `SendPasswordResetEmailCommand` | Identity | Notification | Password reset |
| `SendInquiryNotificationCommand` | Services | Notification | Inquiry alert |

---

## Appendix B: Project Structure

### Final Solution Structure
```
SoftwareConsultingPlatform/
├── src/
│   ├── Aspire/
│   │   ├── SoftwareConsultingPlatform.AppHost/
│   │   │   ├── Program.cs
│   │   │   └── SoftwareConsultingPlatform.AppHost.csproj
│   │   └── SoftwareConsultingPlatform.ServiceDefaults/
│   │       ├── Extensions.cs
│   │       └── SoftwareConsultingPlatform.ServiceDefaults.csproj
│   ├── Shared/
│   │   ├── Shared.Messages/
│   │   │   ├── Events/
│   │   │   │   ├── Identity/
│   │   │   │   ├── Tenant/
│   │   │   │   ├── Services/
│   │   │   │   ├── CaseStudies/
│   │   │   │   └── Content/
│   │   │   ├── Commands/
│   │   │   └── Shared.Messages.csproj
│   │   ├── Shared.Contracts/
│   │   │   ├── Identity/
│   │   │   ├── Tenant/
│   │   │   ├── Services/
│   │   │   ├── CaseStudies/
│   │   │   ├── Content/
│   │   │   └── Shared.Contracts.csproj
│   │   └── Shared.Core/
│   │       ├── Interfaces/
│   │       ├── Extensions/
│   │       └── Shared.Core.csproj
│   ├── ApiGateway/
│   │   └── SoftwareConsultingPlatform.ApiGateway/
│   │       ├── Program.cs
│   │       ├── appsettings.json
│   │       └── SoftwareConsultingPlatform.ApiGateway.csproj
│   ├── Services/
│   │   ├── Identity/
│   │   │   ├── SoftwareConsultingPlatform.Identity.Api/
│   │   │   ├── SoftwareConsultingPlatform.Identity.Core/
│   │   │   └── SoftwareConsultingPlatform.Identity.Infrastructure/
│   │   ├── Tenant/
│   │   │   ├── SoftwareConsultingPlatform.Tenant.Api/
│   │   │   ├── SoftwareConsultingPlatform.Tenant.Core/
│   │   │   └── SoftwareConsultingPlatform.Tenant.Infrastructure/
│   │   ├── Services/
│   │   │   ├── SoftwareConsultingPlatform.Services.Api/
│   │   │   ├── SoftwareConsultingPlatform.Services.Core/
│   │   │   └── SoftwareConsultingPlatform.Services.Infrastructure/
│   │   ├── CaseStudies/
│   │   │   ├── SoftwareConsultingPlatform.CaseStudies.Api/
│   │   │   ├── SoftwareConsultingPlatform.CaseStudies.Core/
│   │   │   └── SoftwareConsultingPlatform.CaseStudies.Infrastructure/
│   │   ├── Content/
│   │   │   ├── SoftwareConsultingPlatform.Content.Api/
│   │   │   ├── SoftwareConsultingPlatform.Content.Core/
│   │   │   └── SoftwareConsultingPlatform.Content.Infrastructure/
│   │   └── Notification/
│   │       └── SoftwareConsultingPlatform.Notification.Api/
│   └── WebApp/
│       └── Ui/
├── tests/
│   ├── SoftwareConsultingPlatform.Identity.Tests/
│   ├── SoftwareConsultingPlatform.Services.Tests/
│   ├── SoftwareConsultingPlatform.Integration.Tests/
│   └── SoftwareConsultingPlatform.E2E.Tests/
├── docs/
│   └── microservices-migration-roadmap.md
└── SoftwareConsultingPlatform.Microservices.sln
```

---

## Migration Checklist Summary

### Phase 1: Foundation
- [x] Create Aspire AppHost project
- [x] Create ServiceDefaults project
- [x] Configure message broker (RabbitMQ)
- [x] Set up development infrastructure

### Phase 2: Shared Projects
- [x] Create Shared.Messages with all event contracts
- [x] Create Shared.Contracts with DTOs
- [x] Create Shared.Core with common utilities

### Phase 3: API Gateway
- [x] Create YARP-based API Gateway
- [x] Configure route mappings
- [x] Implement authentication passthrough
- [x] Add rate limiting

### Phase 4-8: Service Extraction
- [x] Extract Identity Service
- [x] Extract Tenant Service
- [x] Extract Services Management Service
- [x] Extract Case Studies Service
- [x] Extract Content Management Service
- [x] Create Notification Service

### Phase 9-10: Event Integration
- [x] Configure MassTransit in all services
- [x] Implement event publishing
- [x] Implement event consumers
- [x] Add outbox pattern

### Phase 11: Frontend
- [x] Update API configuration
- [x] Test all flows through gateway

### Phase 12-13: Testing & Deployment
- [x] Complete integration tests
- [x] Set up CI/CD pipelines
- [x] Configure production monitoring

---

**Document maintained by:** Engineering Team
**Last updated:** January 2026
