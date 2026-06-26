using GQ.WebApi.UseCases.Handlers.MeterReading.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListApartmentMeterReadings;

/// <summary>Запрос истории показаний по квартире.</summary>
public sealed record ListApartmentMeterReadingsQuery(Guid ApartmentId): IRequest<ListApartmentMeterReadingsResponse>;

/// <summary>Ответ с историей показаний квартиры.</summary>
public sealed record ListApartmentMeterReadingsResponse(IReadOnlyList<MeterReadingDto> Items);
