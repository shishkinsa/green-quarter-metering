using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Queries;

/// <summary>
/// Проекция статуса сдачи показания по квартире дома.
/// </summary>
public sealed record BuildingMeterReadingStatusReadModel(
    Guid ApartmentId,
    string ApartmentNumber,
    string? OwnerFullName,
    decimal? ReadingValue,
    bool Submitted);

/// <summary>
/// Именованные запросы к показаниям (сложные выборки).
/// </summary>
public interface IMeterReadingQueries
{
    /// <summary>
    /// Возвращает максимальное значение показания по квартире за периоды строго раньше указанного.
    /// </summary>
    Task<decimal?> GetMaxValueBeforePeriodAsync(
        Guid apartmentId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает сводку сдачи показаний по дому за период (все квартиры, LEFT JOIN показаний).
    /// </summary>
    Task<IReadOnlyList<BuildingMeterReadingStatusReadModel>> ListByBuildingAndPeriodAsync(
        Guid buildingId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken);
}
