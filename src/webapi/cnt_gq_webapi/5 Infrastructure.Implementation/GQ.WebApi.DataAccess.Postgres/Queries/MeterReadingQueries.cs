using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Queries;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.DataAccess.Postgres.Queries;

/// <summary>
/// Реализация <see cref="IMeterReadingQueries"/> на EF Core.
/// </summary>
public sealed class MeterReadingQueries(IDbContext db): IMeterReadingQueries
{
    public Task<decimal?> GetMaxValueBeforePeriodAsync(
        Guid apartmentId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken = default)
    {
        return db.MeterReadings
            .AsNoTracking()
            .Where(x => x.ApartmentId == apartmentId
                        && (x.PeriodYear < periodYear
                            || (x.PeriodYear == periodYear && x.PeriodMonth < periodMonth)))
            .Select(x => (decimal?)x.Value)
            .MaxAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BuildingMeterReadingStatusReadModel>> ListByBuildingAndPeriodAsync(
        Guid buildingId,
        int periodYear,
        int periodMonth,
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

        Dictionary<Guid, MeterReading> readings = await db.MeterReadings
            .AsNoTracking()
            .Where(x => apartmentIds.Contains(x.ApartmentId)
                        && x.PeriodYear == periodYear
                        && x.PeriodMonth == periodMonth)
            .ToDictionaryAsync(x => x.ApartmentId, cancellationToken);

        List<BuildingMeterReadingStatusReadModel> result = new(apartments.Count);
        foreach(Apartment apartment in apartments)
        {
            owners.TryGetValue(apartment.Id, out Owner? owner);
            readings.TryGetValue(apartment.Id, out MeterReading? reading);
            result.Add(new BuildingMeterReadingStatusReadModel(
                apartment.Id,
                apartment.Number,
                owner?.FullName,
                reading?.Value,
                reading is not null));
        }

        return result;
    }
}
