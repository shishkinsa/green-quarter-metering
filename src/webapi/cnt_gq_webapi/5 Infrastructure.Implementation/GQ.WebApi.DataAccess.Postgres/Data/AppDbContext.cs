using Microsoft.EntityFrameworkCore;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;

namespace GQ.WebApi.DataAccess.Postgres.Data;

/// <summary>
/// EF Core контекст PostgreSQL.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IDbContext
{
    public DbSet<ExampleItem> ExampleItems => Set<ExampleItem>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExampleItem>(entity =>
        {
            entity.ToTable("example_items");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(256).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Code).HasMaxLength(64).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(256).IsRequired();
        });
    }
}
