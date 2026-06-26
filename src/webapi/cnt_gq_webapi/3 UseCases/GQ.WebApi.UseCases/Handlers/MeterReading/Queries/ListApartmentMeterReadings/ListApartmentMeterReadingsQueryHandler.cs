using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

using MeterReadingEntity = GQ.WebApi.Entities.MeterReading;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListApartmentMeterReadings;

/// <summary>
/// Возвращает историю показаний квартиры.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
public sealed class ListApartmentMeterReadingsQueryHandler(IDbContext db)
    : IRequestHandler<ListApartmentMeterReadingsQuery, ListApartmentMeterReadingsResponse>
{
    public async Task<ListApartmentMeterReadingsResponse> Handle(
        ListApartmentMeterReadingsQuery query,
        CancellationToken cancellationToken)
    {
        _ = await db.Apartments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.ApartmentId, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Apartment '{query.ApartmentId}' was not found.");

        List<MeterReadingEntity> items = await db.MeterReadings
            .AsNoTracking()
            .Where(x => x.ApartmentId == query.ApartmentId)
            .OrderByDescending(x => x.PeriodYear)
            .ThenByDescending(x => x.PeriodMonth)
            .ToListAsync(cancellationToken);

        return new ListApartmentMeterReadingsResponse([.. items.Select(MeterReadingMappings.ToDto)]);
    }
}
