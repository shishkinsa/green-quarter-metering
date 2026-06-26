using GQ.WebApi.UseCases.Handlers.Building.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Queries.ListApartmentsWithOwners;

/// <summary>Запрос списка квартир дома вместе с владельцами.</summary>
public sealed record ListApartmentsWithOwnersQuery(Guid BuildingId): IRequest<ListApartmentsWithOwnersResponse>;

/// <summary>Ответ со списком квартир и владельцев.</summary>
public sealed record ListApartmentsWithOwnersResponse(IReadOnlyList<ApartmentWithOwnerDto> Items);
