using MassTransit;
using SoftwareConsultingPlatform.Content.Infrastructure.Data;
using Shared.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddSqlServerDbContext<ContentDbContext>("content-db");
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = builder.Configuration.GetConnectionString("cache"));

builder.Services.AddHttpClient("services-service", c => c.BaseAddress = new Uri("https+http://services-service"));
builder.Services.AddHttpClient("casestudies-service", c => c.BaseAddress = new Uri("https+http://casestudies-service"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    x.SetKebabCaseEndpointNameFormatter();
    x.AddEntityFrameworkOutbox<ContentDbContext>(o => { o.UseSqlServer(); o.UseBusOutbox(); });
    x.UsingRabbitMq((ctx, cfg) =>
    {
        var cs = builder.Configuration.GetConnectionString("messaging");
        if (!string.IsNullOrEmpty(cs)) cfg.Host(new Uri(cs));
        cfg.UseMessageRetry(r => r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5)));
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<ContentDbContext>().Database.EnsureCreated();
}
app.Run();

public class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _hca;
    public TenantContext(IHttpContextAccessor hca) => _hca = hca;
    public Guid TenantId
    {
        get
        {
            var c = _hca.HttpContext?.User?.FindFirst("tenant_id");
            if (c != null && Guid.TryParse(c.Value, out var id)) return id;
            var h = _hca.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(h) && Guid.TryParse(h, out id)) return id;
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }
    }
}
