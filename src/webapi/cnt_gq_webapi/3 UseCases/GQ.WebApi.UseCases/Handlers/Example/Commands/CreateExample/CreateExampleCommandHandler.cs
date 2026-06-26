using FluentValidation;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Responses;
using GQ.WebApi.UseCases.Handlers.Example.Mappings;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample;

/// <summary>
/// Обработчик команды <see cref="CreateExampleCommand"/>.
/// </summary>
public sealed class CreateExampleCommandHandler(
    IExampleItemRepository repository,
    IValidator<CreateExampleCommand> validator)
    : IRequestHandler<CreateExampleCommand, CreateExampleResponse>
{
    public async Task<CreateExampleResponse> Handle(
        CreateExampleCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var item = ExampleItem.Create(command.Name);
        await repository.AddAsync(item, cancellationToken);
        return new CreateExampleResponse(ExampleMappings.ToDto(item));
    }
}
