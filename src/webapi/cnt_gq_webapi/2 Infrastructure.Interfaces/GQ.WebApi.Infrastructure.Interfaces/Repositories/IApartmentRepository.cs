using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

/// <summary>
/// Проекция квартиры с данными владельца для списков справочника.
/// </summary>
public sealed record ApartmentWithOwnerReadModel(
    Guid Id,
    Guid BuildingId,
    string Number,
    int? Floor,
    Guid? OwnerId,
    string? OwnerFullName,
    string? OwnerPhone,
    DateTimeOffset? LastReadingSubmittedAt,
    decimal? LastReadingValue,
    bool CurrentPeriodSubmitted);

/// <summary>
/// Контракт чтения и записи квартир (<see cref="Apartment"/>).
/// </summary>
public interface IApartmentRepository
{
    /// <summary>Возвращает квартиры дома вместе с владельцами, отсортированные по номеру.</summary>
    Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken);

    /// <summary>Возвращает квартиру по идентификатору или <c>null</c>, если не найдена.</summary>
    Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет уникальность номера квартиры в пределах дома.
    /// </summary>
    /// <param name="excludeApartmentId">Идентификатор квартиры, исключаемой из проверки (при обновлении).</param>
    Task<bool> ExistsByBuildingAndNumberAsync(
        Guid buildingId,
        string number,
        Guid? excludeApartmentId,
        CancellationToken cancellationToken);

    /// <summary>Сохраняет новую квартиру.</summary>
    Task AddAsync(Apartment apartment, CancellationToken cancellationToken);

    /// <summary>Возвращает идентификаторы квартир дома.</summary>
    Task<IReadOnlyList<Guid>> ListIdsByBuildingAsync(Guid buildingId, CancellationToken cancellationToken);

    /// <summary>Удаляет квартиру.</summary>
    Task DeleteAsync(Apartment apartment, CancellationToken cancellationToken);
}
