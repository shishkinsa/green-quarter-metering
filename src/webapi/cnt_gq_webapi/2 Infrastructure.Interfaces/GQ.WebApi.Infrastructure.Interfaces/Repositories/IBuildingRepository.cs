using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

public interface IBuildingRepository
{
    Task<IReadOnlyList<Building>> ListAsync(CancellationToken cancellationToken);

    Task<Building?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(Building building, CancellationToken cancellationToken);

    Task UpdateAsync(Building building, CancellationToken cancellationToken);
}
