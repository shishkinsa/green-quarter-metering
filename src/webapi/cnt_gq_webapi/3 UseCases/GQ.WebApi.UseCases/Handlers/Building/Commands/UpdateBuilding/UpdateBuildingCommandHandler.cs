using FluentValidation;

using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding;

/// <summary>
/// Обновляет данные существующего дома.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом с указанным идентификатором не найден.</exception>
public sealed class UpdateBuildingCommandHandler(
    IDbContext db,
    IValidator<UpdateBuildingCommand> validator)
    : IRequestHandler<UpdateBuildingCommand, UpdateBuildingResponse>
{
    public async Task<UpdateBuildingResponse> Handle(
        UpdateBuildingCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        Entities.Building building = await db.Buildings.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Building '{command.Id}' was not found.");

        building.Update(command.Name, command.Address);
        await db.SaveChangesAsync(cancellationToken);
        return new UpdateBuildingResponse(DirectoryMappings.ToDto(building));
    }
}
