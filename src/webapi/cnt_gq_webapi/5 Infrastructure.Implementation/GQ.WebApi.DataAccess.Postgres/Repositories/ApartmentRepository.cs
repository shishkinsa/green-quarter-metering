using Microsoft.EntityFrameworkCore;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.DataAccess.Postgres.Repositories;

/// <summary>
/// Реализация <see cref="IApartmentRepository"/> на EF Core и PostgreSQL.
/// </summary>
public sealed class ApartmentRepository(AppDbContext dbContext) : IApartmentRepository
{
    public async Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Apartments
            .AsNoTracking()
            .Where(x => x.BuildingId == buildingId)
            .OrderBy(x => x.Number)
            .GroupJoin(
                dbContext.Owners.AsNoTracking(),
                apartment => apartment.Id,
                owner => owner.ApartmentId,
                (apartment, owners) => new { apartment, owner = owners.FirstOrDefault() })
            .Select(x => new ApartmentWithOwnerReadModel(
                x.apartment.Id,
                x.apartment.BuildingId,
                x.apartment.Number,
                x.apartment.Floor,
                x.owner != null ? x.owner.Id : (Guid?)null,
                x.owner != null ? x.owner.FullName : null,
                x.owner != null ? x.owner.Phone : null))
            .ToListAsync(cancellationToken);
    }

    public Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Apartments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByBuildingAndNumberAsync(
        Guid buildingId,
        string number,
        Guid? excludeApartmentId,
        CancellationToken cancellationToken = default)
    {
        var normalized = number.Trim();
        return dbContext.Apartments.AnyAsync(
            x => x.BuildingId == buildingId
                 && x.Number == normalized
                 && (excludeApartmentId == null || x.Id != excludeApartmentId),
            cancellationToken);
    }

    public async Task AddAsync(Apartment apartment, CancellationToken cancellationToken = default)
    {
        dbContext.Apartments.Add(apartment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
