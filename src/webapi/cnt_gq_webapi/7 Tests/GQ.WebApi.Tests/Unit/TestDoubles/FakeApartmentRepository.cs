using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class FakeApartmentRepository(FakeOwnerRepository owners): IApartmentRepository
{
    public List<Apartment> Items { get; } = [];

    public Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ApartmentWithOwnerReadModel> items = Items
            .Where(x => x.BuildingId == buildingId)
            .OrderBy(x => x.Number)
            .Select(apartment =>
            {
                Owner? owner = owners.Items.FirstOrDefault(x => x.ApartmentId == apartment.Id);
                return new ApartmentWithOwnerReadModel(
                    apartment.Id,
                    apartment.BuildingId,
                    apartment.Number,
                    apartment.Floor,
                    owner?.Id,
                    owner?.FullName,
                    owner?.Phone);
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
}
