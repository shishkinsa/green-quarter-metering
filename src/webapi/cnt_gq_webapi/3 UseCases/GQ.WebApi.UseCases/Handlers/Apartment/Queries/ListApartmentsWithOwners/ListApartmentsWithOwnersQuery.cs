using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Queries.ListApartmentsWithOwners;

public sealed record ListApartmentsWithOwnersQuery(Guid BuildingId) : IRequest<ListApartmentsWithOwnersResponse>;

public sealed record ListApartmentsWithOwnersResponse(IReadOnlyList<ApartmentWithOwnerDto> Items);
