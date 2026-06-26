using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

/// <summary>
/// Контракт чтения и записи владельцев квартир (<see cref="Owner"/>).
/// </summary>
public interface IOwnerRepository
{
    /// <summary>Возвращает владельца квартиры или <c>null</c>, если не назначен.</summary>
    Task<Owner?> GetByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken);

    /// <summary>Сохраняет нового владельца.</summary>
    Task AddAsync(Owner owner, CancellationToken cancellationToken);

    /// <summary>Сохраняет изменения существующего владельца.</summary>
    Task UpdateAsync(Owner owner, CancellationToken cancellationToken);
}
