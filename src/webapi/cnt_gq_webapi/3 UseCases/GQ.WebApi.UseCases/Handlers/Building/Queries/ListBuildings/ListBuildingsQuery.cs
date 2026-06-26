using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Queries.ListBuildings;

/// <summary>Запрос списка всех домов справочника.</summary>
public sealed record ListBuildingsQuery : IRequest<ListBuildingsResponse>;

/// <summary>Ответ со списком домов.</summary>
public sealed record ListBuildingsResponse(IReadOnlyList<BuildingDto> Items);
