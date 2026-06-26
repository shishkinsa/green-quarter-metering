using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Mappings;

using MediatR;

using MeterReadingEntity = GQ.WebApi.Entities.MeterReading;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListApartmentMeterReadings;

/// <summary>
/// Возвращает историю показаний квартиры.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
public sealed class ListApartmentMeterReadingsQueryHandler(
    IApartmentRepository apartmentRepository,
    IMeterReadingRepository meterReadingRepository)
    : IRequestHandler<ListApartmentMeterReadingsQuery, ListApartmentMeterReadingsResponse>
{
    public async Task<ListApartmentMeterReadingsResponse> Handle(
        ListApartmentMeterReadingsQuery query,
        CancellationToken cancellationToken)
    {
        _ = await apartmentRepository.GetByIdAsync(query.ApartmentId, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Apartment '{query.ApartmentId}' was not found.");

        IReadOnlyList<MeterReadingEntity> items = await meterReadingRepository.ListByApartmentAsync(
            query.ApartmentId,
            cancellationToken);

        return new ListApartmentMeterReadingsResponse(items.Select(MeterReadingMappings.ToDto).ToList());
    }
}
