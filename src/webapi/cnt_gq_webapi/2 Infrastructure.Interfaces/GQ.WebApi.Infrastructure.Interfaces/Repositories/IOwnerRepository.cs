using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

public interface IOwnerRepository
{
    Task<Owner?> GetByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken);

    Task AddAsync(Owner owner, CancellationToken cancellationToken);

    Task UpdateAsync(Owner owner, CancellationToken cancellationToken);
}
