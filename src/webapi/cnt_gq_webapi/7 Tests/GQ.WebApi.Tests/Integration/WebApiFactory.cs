using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.DataAccess.Postgres.Repositories;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace GQ.WebApi.Tests.Integration;

/// <summary>
/// Фабрика тестового хоста с InMemory БД.
/// </summary>
public sealed class WebApiFactory: WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"WebApiTests_{Guid.NewGuid():N}";

    public WebApiFactory()
    {
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", string.Empty);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = string.Empty,
                ["Database:AutoMigrate"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(AppDbContext));
            services.RemoveAll(typeof(IDbContext));
            services.RemoveAll(typeof(IBuildingRepository));
            services.RemoveAll(typeof(IApartmentRepository));
            services.RemoveAll(typeof(IOwnerRepository));

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
            services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        IHost host = base.CreateHost(builder);

        using IServiceScope scope = host.Services.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IntegrationTestDatabase.Seed(db);

        return host;
    }
}
