using FluentValidation;

using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

using BuildingEntity = GQ.WebApi.Entities.Building;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;

/// <summary>Создаёт дом в справочнике ЖК.</summary>
public sealed class CreateBuildingCommandHandler(
    IDbContext db,
    IValidator<CreateBuildingCommand> validator)
    : IRequestHandler<CreateBuildingCommand, CreateBuildingResponse>
{
    public async Task<CreateBuildingResponse> Handle(
        CreateBuildingCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        BuildingEntity building = BuildingEntity.Create(command.Name, command.Address);
        db.Buildings.Add(building);
        await db.SaveChangesAsync(cancellationToken);
        return new CreateBuildingResponse(DirectoryMappings.ToDto(building));
    }
}
