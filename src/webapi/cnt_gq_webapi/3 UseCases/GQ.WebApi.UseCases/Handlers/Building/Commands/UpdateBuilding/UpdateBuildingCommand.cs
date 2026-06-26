using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding;

public sealed record UpdateBuildingCommand(Guid Id, string Name, string? Address)
    : IRequest<UpdateBuildingResponse>;

public sealed record UpdateBuildingResponse(BuildingDto Item);
