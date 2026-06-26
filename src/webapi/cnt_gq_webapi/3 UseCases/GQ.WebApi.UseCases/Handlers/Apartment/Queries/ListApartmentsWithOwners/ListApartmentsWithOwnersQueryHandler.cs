using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Queries.ListApartmentsWithOwners;

/// <summary>
/// Возвращает квартиры дома вместе с данными владельцев.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
public sealed class ListApartmentsWithOwnersQueryHandler(
    IBuildingRepository buildingRepository,
    IApartmentRepository apartmentRepository)
    : IRequestHandler<ListApartmentsWithOwnersQuery, ListApartmentsWithOwnersResponse>
{
    public async Task<ListApartmentsWithOwnersResponse> Handle(
        ListApartmentsWithOwnersQuery query,
        CancellationToken cancellationToken)
    {
        var building = await buildingRepository.GetByIdAsync(query.BuildingId, cancellationToken);
        if (building is null)
        {
            throw new UseCaseNotFoundException($"Building '{query.BuildingId}' was not found.");
        }

        var items = await apartmentRepository.ListByBuildingWithOwnersAsync(query.BuildingId, cancellationToken);
        return new ListApartmentsWithOwnersResponse(items.Select(DirectoryMappings.ToDto).ToList());
    }
}
