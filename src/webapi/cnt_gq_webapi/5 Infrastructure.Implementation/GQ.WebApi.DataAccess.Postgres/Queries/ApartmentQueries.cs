using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Queries;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.DataAccess.Postgres.Queries;

/// <summary>
/// Реализация <see cref="IApartmentQueries"/> на EF Core.
/// </summary>
public sealed class ApartmentQueries(IDbContext db): IApartmentQueries
{
    public async Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        List<Apartment> apartments = await db.Apartments
            .AsNoTracking()
            .Where(x => x.BuildingId == buildingId)
            .OrderBy(x => x.Number)
            .ToListAsync(cancellationToken);

        if(apartments.Count == 0)
        {
            return [];
        }

        HashSet<Guid> apartmentIds = [.. apartments.Select(x => x.Id)];

        Dictionary<Guid, Owner> owners = await db.Owners
            .AsNoTracking()
            .Where(x => apartmentIds.Contains(x.ApartmentId))
            .ToDictionaryAsync(x => x.ApartmentId, cancellationToken);

        List<MeterReading> readings = await db.MeterReadings
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
}
