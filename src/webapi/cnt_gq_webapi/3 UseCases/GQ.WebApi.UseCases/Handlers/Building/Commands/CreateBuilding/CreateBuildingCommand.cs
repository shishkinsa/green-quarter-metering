using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;

public sealed record CreateBuildingCommand(string Name, string? Address) : IRequest<CreateBuildingResponse>;

public sealed record CreateBuildingResponse(BuildingDto Item);
