using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class FakeMeterReadingRepository(
    FakeApartmentRepository apartments,
    FakeOwnerRepository owners): IMeterReadingRepository
{
    public List<MeterReading> Items { get; } = [];

    public Task<MeterReading?> GetByApartmentAndPeriodAsync(
        Guid apartmentId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            Items.FirstOrDefault(
                x => x.ApartmentId == apartmentId
                     && x.PeriodYear == periodYear
                     && x.PeriodMonth == periodMonth));
    }

    public Task<decimal?> GetMaxValueBeforePeriodAsync(
        Guid apartmentId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken = default)
    {
        decimal? max = Items
            .Where(
                x => x.ApartmentId == apartmentId
                     && (x.PeriodYear < periodYear
                         || (x.PeriodYear == periodYear && x.PeriodMonth < periodMonth)))
            .Select(x => (decimal?)x.Value)
            .DefaultIfEmpty()
            .Max();

        return Task.FromResult(max);
    }

    public Task AddAsync(MeterReading meterReading, CancellationToken cancellationToken = default)
    {
        Items.Add(meterReading);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(MeterReading meterReading, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<BuildingMeterReadingStatusReadModel>> ListByBuildingAndPeriodAsync(
        Guid buildingId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken = default)
    {
        List<BuildingMeterReadingStatusReadModel> result = apartments.Items
            .Where(x => x.BuildingId == buildingId)
            .OrderBy(x => x.Number)
            .Select(apartment =>
            {
                Owner? owner = owners.Items.FirstOrDefault(x => x.ApartmentId == apartment.Id);
                MeterReading? reading = Items.FirstOrDefault(
                    x => x.ApartmentId == apartment.Id
                         && x.PeriodYear == periodYear
                         && x.PeriodMonth == periodMonth);

                return new BuildingMeterReadingStatusReadModel(
                    apartment.Id,
                    apartment.Number,
                    owner?.FullName,
                    reading?.Value,
                    reading is not null);
            })
            .ToList();

        return Task.FromResult<IReadOnlyList<BuildingMeterReadingStatusReadModel>>(result);
    }

    public Task<IReadOnlyList<MeterReading>> ListByApartmentAsync(
        Guid apartmentId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<MeterReading> items = Items
            .Where(x => x.ApartmentId == apartmentId)
            .OrderByDescending(x => x.PeriodYear)
            .ThenByDescending(x => x.PeriodMonth)
            .ToList();

        return Task.FromResult(items);
    }

    public Task DeleteByApartmentAsync(Guid apartmentId, CancellationToken cancellationToken = default)
    {
        Items.RemoveAll(x => x.ApartmentId == apartmentId);
        return Task.CompletedTask;
    }
}
