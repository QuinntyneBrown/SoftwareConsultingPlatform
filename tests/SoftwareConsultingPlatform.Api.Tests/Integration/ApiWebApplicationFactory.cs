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

            // Add InMemory database for testing
            services.AddDbContext<SoftwareConsultingPlatformContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });
            
            services.AddScoped<ISoftwareConsultingPlatformContext>(sp => 
                sp.GetRequiredService<SoftwareConsultingPlatformContext>());
        });
    }
}
