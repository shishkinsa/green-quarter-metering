using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

/// <summary>
/// Контракт персистентности для <see cref="ExampleItem"/>.
/// </summary>
public interface IExampleItemRepository
{
    Task AddAsync(ExampleItem item, CancellationToken cancellationToken = default);

    Task<ExampleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExampleItem>> ListAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(ExampleItem item, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
