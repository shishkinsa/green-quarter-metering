using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Queries.ListBuildings;

public sealed record ListBuildingsQuery : IRequest<ListBuildingsResponse>;

public sealed record ListBuildingsResponse(IReadOnlyList<BuildingDto> Items);
