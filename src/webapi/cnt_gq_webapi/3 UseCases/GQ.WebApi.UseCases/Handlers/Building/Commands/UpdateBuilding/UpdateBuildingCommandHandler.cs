using FluentValidation;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding;

/// <summary>
/// Обновляет данные существующего дома.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом с указанным идентификатором не найден.</exception>
public sealed class UpdateBuildingCommandHandler(
    IBuildingRepository repository,
    IValidator<UpdateBuildingCommand> validator)
    : IRequestHandler<UpdateBuildingCommand, UpdateBuildingResponse>
{
    public async Task<UpdateBuildingResponse> Handle(
        UpdateBuildingCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var building = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (building is null)
        {
            throw new UseCaseNotFoundException($"Building '{command.Id}' was not found.");
        }

        building.Update(command.Name, command.Address);
        await repository.UpdateAsync(building, cancellationToken);
        return new UpdateBuildingResponse(DirectoryMappings.ToDto(building));
    }
}
