using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Core.Extensions;

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
                var connectionString = configuration.GetConnectionString("messaging");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    cfg.Host(new Uri(connectionString));
                }

                cfg.UseMessageRetry(r => r.Exponential(
                    5,
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromSeconds(5)));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
