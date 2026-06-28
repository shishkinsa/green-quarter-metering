using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.DataAccess.Postgres.Data;

/// <summary>
/// EF Core контекст PostgreSQL.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options), IDbContext
{
    public DbSet<Building> Buildings => Set<Building>();

    public DbSet<Apartment> Apartments => Set<Apartment>();

    public DbSet<Owner> Owners => Set<Owner>();

    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.ToTable("buildings");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Address).HasMaxLength(512);
        });

        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.ToTable("apartments");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Number).HasMaxLength(32).IsRequired();
            entity.Property(x => x.MeterVerificationDate);
            entity.HasIndex(x => new { x.BuildingId, x.Number }).IsUnique();
            entity.HasOne<Building>()
                .WithMany()
                .HasForeignKey(x => x.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.ToTable("owners");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(32);
            entity.HasIndex(x => x.ApartmentId).IsUnique();
            entity.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(x => x.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MeterReading>(entity =>
        {
            entity.ToTable("meter_readings");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Value).HasPrecision(18, 3);
            entity.Property(x => x.SubmittedAt).IsRequired();
            entity.HasIndex(x => new { x.ApartmentId, x.PeriodYear, x.PeriodMonth }).IsUnique();
            entity.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(x => x.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
