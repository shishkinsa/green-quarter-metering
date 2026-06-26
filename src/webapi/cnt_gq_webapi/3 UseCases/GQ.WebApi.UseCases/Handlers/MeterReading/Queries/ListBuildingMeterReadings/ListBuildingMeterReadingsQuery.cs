using GQ.WebApi.UseCases.Handlers.MeterReading.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListBuildingMeterReadings;

/// <summary>Запрос сводки сдачи показаний по дому за период.</summary>
public sealed record ListBuildingMeterReadingsQuery(
    Guid BuildingId,
    int PeriodYear,
    int PeriodMonth): IRequest<ListBuildingMeterReadingsResponse>;

/// <summary>Ответ со сводкой сдачи показаний.</summary>
public sealed record ListBuildingMeterReadingsResponse(IReadOnlyList<BuildingMeterReadingStatusDto> Items);
