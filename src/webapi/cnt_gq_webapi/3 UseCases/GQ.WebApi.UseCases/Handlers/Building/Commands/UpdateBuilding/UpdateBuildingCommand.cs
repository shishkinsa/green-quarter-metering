using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding;

/// <summary>Команда обновления данных дома.</summary>
public sealed record UpdateBuildingCommand(Guid Id, string Name, string? Address)
    : IRequest<UpdateBuildingResponse>;

/// <summary>Ответ сценария обновления дома.</summary>
public sealed record UpdateBuildingResponse(BuildingDto Item);
