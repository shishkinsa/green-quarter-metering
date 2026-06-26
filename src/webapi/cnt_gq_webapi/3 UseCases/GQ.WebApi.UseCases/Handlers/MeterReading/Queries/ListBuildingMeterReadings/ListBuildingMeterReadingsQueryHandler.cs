using FluentValidation;

using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Queries;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListBuildingMeterReadings;

/// <summary>
/// Возвращает сводку сдачи показаний по дому за отчётный период.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
public sealed class ListBuildingMeterReadingsQueryHandler(
    IDbContext db,
    IMeterReadingQueries meterReadingQueries,
    IValidator<ListBuildingMeterReadingsQuery> validator)
    : IRequestHandler<ListBuildingMeterReadingsQuery, ListBuildingMeterReadingsResponse>
{
    public async Task<ListBuildingMeterReadingsResponse> Handle(
        ListBuildingMeterReadingsQuery query,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        _ = await db.Buildings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.BuildingId, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Building '{query.BuildingId}' was not found.");

        IReadOnlyList<BuildingMeterReadingStatusReadModel> items = await meterReadingQueries.ListByBuildingAndPeriodAsync(
            query.BuildingId,
            query.PeriodYear,
            query.PeriodMonth,
            cancellationToken);

        return new ListBuildingMeterReadingsResponse([.. items.Select(MeterReadingMappings.ToDto)]);
    }
}
