using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

/// <summary>
/// Контракт чтения и записи сущностей <see cref="Building"/>.
/// </summary>
public interface IBuildingRepository
{
    /// <summary>Возвращает все дома, отсортированные по наименованию.</summary>
    Task<IReadOnlyList<Building>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Возвращает дом по идентификатору или <c>null</c>, если не найден.</summary>
    Task<Building?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Сохраняет новый дом.</summary>
    Task AddAsync(Building building, CancellationToken cancellationToken);

    /// <summary>Сохраняет изменения существующего дома.</summary>
    Task UpdateAsync(Building building, CancellationToken cancellationToken);
}
