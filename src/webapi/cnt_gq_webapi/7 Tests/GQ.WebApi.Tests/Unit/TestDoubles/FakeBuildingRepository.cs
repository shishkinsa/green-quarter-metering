using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class FakeBuildingRepository: IBuildingRepository
{
    public List<Building> Items { get; } = [];

    public Task<IReadOnlyList<Building>> ListAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Building> items = Items.OrderBy(x => x.Name).ToList();
        return Task.FromResult(items);
    }

    public Task<Building?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    }

    public Task AddAsync(Building building, CancellationToken cancellationToken = default)
    {
        Items.Add(building);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Building building, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
