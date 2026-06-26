using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.DataAccess.Postgres.Repositories;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.DataAccess.Postgres.DependencyInjection;

/// <summary>
/// Регистрация инфраструктуры доступа к данным PostgreSQL.
/// </summary>
public static class DataAccessServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDataAccess(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IExampleItemRepository, ExampleItemRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
