using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Infrastructure;
using SoftwareConsultingPlatform.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configure database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Database=SoftwareConsultingPlatform;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<SoftwareConsultingPlatformContext>(options =>
    options.UseSqlServer(connectionString));

// Register interfaces
builder.Services.AddScoped<ISoftwareConsultingPlatformContext, SoftwareConsultingPlatformContext>();
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Software Consulting Platform API", Version = "v1" });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost:4200" };
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
