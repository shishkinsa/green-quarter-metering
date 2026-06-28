namespace GQ.WebApi.Infrastructure.Interfaces.Queries;

/// <summary>
/// Проекция квартиры с данными владельца для списков справочника.
/// </summary>
public sealed record ApartmentWithOwnerReadModel(
    Guid Id,
    Guid BuildingId,
    string Number,
    int? Floor,
    DateOnly? MeterVerificationDate,
    Guid? OwnerId,
    string? OwnerFullName,
    string? OwnerPhone,
    DateTimeOffset? LastReadingSubmittedAt,
    decimal? LastReadingValue,
    bool CurrentPeriodSubmitted);

/// <summary>
/// Именованные запросы к данным квартир (сложные выборки).
/// </summary>
public interface IApartmentQueries
{
    /// <summary>Возвращает квартиры дома вместе с владельцами, отсортированные по номеру.</summary>
    Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken);
}
