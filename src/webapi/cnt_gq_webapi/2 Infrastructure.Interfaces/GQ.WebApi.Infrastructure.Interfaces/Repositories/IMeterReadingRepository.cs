using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

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
/// Контракт чтения и записи показаний (<see cref="MeterReading"/>).
/// </summary>
public interface IMeterReadingRepository
{
    /// <summary>Возвращает показание квартиры за период или <c>null</c>.</summary>
    Task<MeterReading?> GetByApartmentAndPeriodAsync(
        Guid apartmentId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает максимальное значение показания по квартире за периоды строго раньше указанного.
    /// </summary>
    Task<decimal?> GetMaxValueBeforePeriodAsync(
        Guid apartmentId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken);

    /// <summary>Сохраняет новое показание.</summary>
    Task AddAsync(MeterReading meterReading, CancellationToken cancellationToken);

    /// <summary>Обновляет существующее показание.</summary>
    Task UpdateAsync(MeterReading meterReading, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает сводку сдачи показаний по дому за период (все квартиры, LEFT JOIN показаний).
    /// </summary>
    Task<IReadOnlyList<BuildingMeterReadingStatusReadModel>> ListByBuildingAndPeriodAsync(
        Guid buildingId,
        int periodYear,
        int periodMonth,
        CancellationToken cancellationToken);
}
