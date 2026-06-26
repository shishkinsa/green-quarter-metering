using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Queries;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Queries.ListApartmentsWithOwners;

/// <summary>
/// Возвращает квартиры дома вместе с данными владельцев.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
public sealed class ListApartmentsWithOwnersQueryHandler(
    IDbContext db,
    IApartmentQueries apartmentQueries)
    : IRequestHandler<ListApartmentsWithOwnersQuery, ListApartmentsWithOwnersResponse>
{
    public async Task<ListApartmentsWithOwnersResponse> Handle(
        ListApartmentsWithOwnersQuery query,
        CancellationToken cancellationToken)
    {
        _ = await db.Buildings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.BuildingId, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Building '{query.BuildingId}' was not found.");

        IReadOnlyList<ApartmentWithOwnerReadModel> items = await apartmentQueries.ListByBuildingWithOwnersAsync(
            query.BuildingId,
            cancellationToken);
        return new ListApartmentsWithOwnersResponse([.. items.Select(DirectoryMappings.ToDto)]);
    }
}
