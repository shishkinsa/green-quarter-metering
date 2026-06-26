using GQ.WebApi.Entities;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Infrastructure.Interfaces.DataAccess;

/// <summary>
/// Абстракция контекста данных (Unit of Work) для UseCases.
/// </summary>
public interface IDbContext
{
    DbSet<Building> Buildings { get; }

    DbSet<Apartment> Apartments { get; }

    DbSet<Owner> Owners { get; }

    DbSet<MeterReading> MeterReadings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
