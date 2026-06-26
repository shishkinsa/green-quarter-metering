using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class FakeApartmentRepository(FakeOwnerRepository owners): IApartmentRepository
{
    public FakeMeterReadingRepository? MeterReadings { get; set; }

    public List<Apartment> Items { get; } = [];

    public Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        DateTime utcNow = DateTime.UtcNow;
        int currentYear = utcNow.Year;
        int currentMonth = utcNow.Month;

        IReadOnlyList<ApartmentWithOwnerReadModel> items = Items
            .Where(x => x.BuildingId == buildingId)
            .OrderBy(x => x.Number)
            .Select(apartment =>
            {
                Owner? owner = owners.Items.FirstOrDefault(x => x.ApartmentId == apartment.Id);
                List<MeterReading> readings = MeterReadings?.Items
                    .Where(x => x.ApartmentId == apartment.Id)
                    .ToList() ?? [];

                MeterReading? lastReading = readings
                    .OrderByDescending(x => x.PeriodYear)
                    .ThenByDescending(x => x.PeriodMonth)
                    .FirstOrDefault();

                bool currentPeriodSubmitted = readings.Any(
                    x => x.PeriodYear == currentYear && x.PeriodMonth == currentMonth);

                return new ApartmentWithOwnerReadModel(
                    apartment.Id,
                    apartment.BuildingId,
                    apartment.Number,
                    apartment.Floor,
                    owner?.Id,
                    owner?.FullName,
                    owner?.Phone,
                    lastReading?.SubmittedAt,
                    lastReading?.Value,
                    currentPeriodSubmitted);
            })
            .ToList();

        return Task.FromResult(items);
    }

    public Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    }

    public Task<bool> ExistsByBuildingAndNumberAsync(
        Guid buildingId,
        string number,
        Guid? excludeApartmentId,
        CancellationToken cancellationToken = default)
    {
        string normalized = number.Trim();
        bool exists = Items.Any(
            x => x.BuildingId == buildingId
                 && x.Number == normalized
                 && (excludeApartmentId is null || x.Id != excludeApartmentId));
        return Task.FromResult(exists);
    }

    public Task AddAsync(Apartment apartment, CancellationToken cancellationToken = default)
    {
        Items.Add(apartment);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Guid>> ListIdsByBuildingAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Guid> ids = Items.Where(x => x.BuildingId == buildingId).Select(x => x.Id).ToList();
        return Task.FromResult(ids);
    }

    public Task DeleteAsync(Apartment apartment, CancellationToken cancellationToken = default)
    {
        Items.Remove(apartment);
        owners.Items.RemoveAll(x => x.ApartmentId == apartment.Id);
        return Task.CompletedTask;
    }
}
