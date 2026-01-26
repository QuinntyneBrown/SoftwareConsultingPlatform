var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var rabbitMq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithDataVolume("rabbitmq-data");

var redis = builder.AddRedis("cache")
    .WithDataVolume("redis-data");

// SQL Server - use existing local instance (ARM64 compatible)
// Connection strings reference databases on the local SQL Server
var identityDb = builder.AddConnectionString("identity-db");
var tenantDb = builder.AddConnectionString("tenant-db");
var servicesDb = builder.AddConnectionString("services-db");
var caseStudiesDb = builder.AddConnectionString("casestudies-db");
var contentDb = builder.AddConnectionString("content-db");

// Identity Service
var identityService = builder.AddProject<Projects.SoftwareConsultingPlatform_Identity_Api>("identity-service")
    .WithReference(identityDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

// Tenant Service
var tenantService = builder.AddProject<Projects.SoftwareConsultingPlatform_Tenant_Api>("tenant-service")
    .WithReference(tenantDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

// Services Management Service
var servicesService = builder.AddProject<Projects.SoftwareConsultingPlatform_Services_Api>("services-service")
    .WithReference(servicesDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(tenantService);

// Case Studies Service
var caseStudiesService = builder.AddProject<Projects.SoftwareConsultingPlatform_CaseStudies_Api>("casestudies-service")
    .WithReference(caseStudiesDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(tenantService);

// Content Service
var contentService = builder.AddProject<Projects.SoftwareConsultingPlatform_Content_Api>("content-service")
    .WithReference(contentDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(servicesService)
    .WithReference(caseStudiesService)
    .WithReference(tenantService);

// Notification Service
var notificationService = builder.AddProject<Projects.SoftwareConsultingPlatform_Notification_Api>("notification-service")
    .WithReference(rabbitMq);

// API Gateway
var apiGateway = builder.AddProject<Projects.SoftwareConsultingPlatform_ApiGateway>("api-gateway")
    .WithReference(identityService)
    .WithReference(tenantService)
    .WithReference(servicesService)
    .WithReference(caseStudiesService)
    .WithReference(contentService)
    .WithExternalHttpEndpoints();

// Frontend
builder.AddNpmApp("ui", "../Ui")
    .WithReference(apiGateway)
    .WaitFor(apiGateway)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
