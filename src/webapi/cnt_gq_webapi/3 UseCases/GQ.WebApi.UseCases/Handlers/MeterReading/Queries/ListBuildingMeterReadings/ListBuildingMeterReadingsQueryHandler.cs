using FluentValidation;

using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Mappings;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListBuildingMeterReadings;

/// <summary>
/// Возвращает сводку сдачи показаний по дому за отчётный период.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
public sealed class ListBuildingMeterReadingsQueryHandler(
    IBuildingRepository buildingRepository,
    IMeterReadingRepository meterReadingRepository,
    IValidator<ListBuildingMeterReadingsQuery> validator)
    : IRequestHandler<ListBuildingMeterReadingsQuery, ListBuildingMeterReadingsResponse>
{
    public async Task<ListBuildingMeterReadingsResponse> Handle(
        ListBuildingMeterReadingsQuery query,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        Entities.Building building = await buildingRepository.GetByIdAsync(query.BuildingId, cancellationToken) ?? throw new UseCaseNotFoundException($"Building '{query.BuildingId}' was not found.");

        IReadOnlyList<BuildingMeterReadingStatusReadModel> items = await meterReadingRepository.ListByBuildingAndPeriodAsync(
            query.BuildingId,
            query.PeriodYear,
            query.PeriodMonth,
            cancellationToken);

        return new ListBuildingMeterReadingsResponse(items.Select(MeterReadingMappings.ToDto).ToList());
    }
}
