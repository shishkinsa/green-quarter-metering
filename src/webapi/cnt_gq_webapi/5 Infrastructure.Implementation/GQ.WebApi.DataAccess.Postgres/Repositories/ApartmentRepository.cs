using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.DataAccess.Postgres.Repositories;

/// <summary>
/// Реализация <see cref="IApartmentRepository"/> на EF Core и PostgreSQL.
/// </summary>
public sealed class ApartmentRepository(AppDbContext dbContext): IApartmentRepository
{
    public async Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        List<Apartment> apartments = await dbContext.Apartments
            .AsNoTracking()
            .Where(x => x.BuildingId == buildingId)
            .OrderBy(x => x.Number)
            .ToListAsync(cancellationToken);

        if(apartments.Count == 0)
        {
            return [];
        }

        HashSet<Guid> apartmentIds = [.. apartments.Select(x => x.Id)];

        Dictionary<Guid, Owner> owners = await dbContext.Owners
            .AsNoTracking()
            .Where(x => apartmentIds.Contains(x.ApartmentId))
            .ToDictionaryAsync(x => x.ApartmentId, cancellationToken);

        List<MeterReading> readings = await dbContext.MeterReadings
            .AsNoTracking()
            .Where(x => apartmentIds.Contains(x.ApartmentId))
            .ToListAsync(cancellationToken);

        DateTime utcNow = DateTime.UtcNow;
        int currentYear = utcNow.Year;
        int currentMonth = utcNow.Month;

        List<ApartmentWithOwnerReadModel> result = new(apartments.Count);
        foreach(Apartment apartment in apartments)
        {
            owners.TryGetValue(apartment.Id, out Owner? owner);
            List<MeterReading> apartmentReadings = readings
                .Where(x => x.ApartmentId == apartment.Id)
                .ToList();

            MeterReading? lastReading = apartmentReadings
                .OrderByDescending(x => x.PeriodYear)
                .ThenByDescending(x => x.PeriodMonth)
                .FirstOrDefault();

            bool currentPeriodSubmitted = apartmentReadings.Any(
                x => x.PeriodYear == currentYear && x.PeriodMonth == currentMonth);

            result.Add(new ApartmentWithOwnerReadModel(
                apartment.Id,
                apartment.BuildingId,
                apartment.Number,
                apartment.Floor,
                owner?.Id,
                owner?.FullName,
                owner?.Phone,
                lastReading?.SubmittedAt,
                lastReading?.Value,
                currentPeriodSubmitted));
        }

        return result;
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
        string normalized = number.Trim();
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

    public async Task<IReadOnlyList<Guid>> ListIdsByBuildingAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Apartments
            .AsNoTracking()
            .Where(x => x.BuildingId == buildingId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Apartment apartment, CancellationToken cancellationToken = default)
    {
        dbContext.Apartments.Remove(apartment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
