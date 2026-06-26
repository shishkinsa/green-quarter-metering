using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;

/// <summary>Команда создания дома в справочнике.</summary>
public sealed record CreateBuildingCommand(string Name, string? Address) : IRequest<CreateBuildingResponse>;

/// <summary>Ответ сценария создания дома.</summary>
public sealed record CreateBuildingResponse(BuildingDto Item);
