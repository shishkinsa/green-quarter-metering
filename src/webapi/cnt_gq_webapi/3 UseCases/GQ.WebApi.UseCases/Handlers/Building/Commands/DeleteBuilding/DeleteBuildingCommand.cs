using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.DeleteBuilding;

/// <summary>Команда удаления дома и связанных данных.</summary>
public sealed record DeleteBuildingCommand(Guid Id): IRequest;
