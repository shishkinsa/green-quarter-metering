using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class FakeOwnerRepository: IOwnerRepository
{
    public List<Owner> Items { get; } = [];

    public Task<Owner?> GetByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Items.FirstOrDefault(x => x.ApartmentId == apartmentId));
    }

    public Task AddAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        Items.Add(owner);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
