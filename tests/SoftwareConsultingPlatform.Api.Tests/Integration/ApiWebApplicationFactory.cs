using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Infrastructure;

namespace SoftwareConsultingPlatform.Api.Tests.Integration;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<SoftwareConsultingPlatformContext>));
            services.RemoveAll(typeof(SoftwareConsultingPlatformContext));
            services.RemoveAll(typeof(ITenantContext));

            // Add InMemory database for testing
            services.AddDbContext<SoftwareConsultingPlatformContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });
            
            services.AddScoped<ISoftwareConsultingPlatformContext>(sp => 
                sp.GetRequiredService<SoftwareConsultingPlatformContext>());
            
            // Add mock tenant context with a default test tenant
            services.AddScoped<ITenantContext>(sp => new TestTenantContext());
        });
    }
}

public class TestTenantContext : ITenantContext
{
    public Guid TenantId { get; } = Guid.Parse("00000000-0000-0000-0000-000000000001");
}

