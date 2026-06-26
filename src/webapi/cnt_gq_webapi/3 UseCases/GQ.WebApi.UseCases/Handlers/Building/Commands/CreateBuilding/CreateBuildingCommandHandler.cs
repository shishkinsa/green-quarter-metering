using FluentValidation;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;
using MediatR;
using BuildingEntity = GQ.WebApi.Entities.Building;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;

public sealed class CreateBuildingCommandHandler(
    IBuildingRepository repository,
    IValidator<CreateBuildingCommand> validator)
    : IRequestHandler<CreateBuildingCommand, CreateBuildingResponse>
{
    public async Task<CreateBuildingResponse> Handle(
        CreateBuildingCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var building = BuildingEntity.Create(command.Name, command.Address);
        await repository.AddAsync(building, cancellationToken);
        return new CreateBuildingResponse(DirectoryMappings.ToDto(building));
    }
}
