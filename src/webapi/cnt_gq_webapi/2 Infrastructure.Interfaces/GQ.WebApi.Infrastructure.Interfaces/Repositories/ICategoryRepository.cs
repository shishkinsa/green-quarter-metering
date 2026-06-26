using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

/// <summary>
/// Контракт read-only доступа к справочнику <see cref="Category"/>.
/// </summary>
public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken = default);
}
