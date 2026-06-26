using FluentValidation;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample.Responses;
using GQ.WebApi.UseCases.Handlers.Example.Mappings;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample;

/// <summary>
/// Обработчик команды <see cref="UpdateExampleCommand"/>.
/// </summary>
public sealed class UpdateExampleCommandHandler(
    IExampleItemRepository repository,
    IValidator<UpdateExampleCommand> validator)
    : IRequestHandler<UpdateExampleCommand, UpdateExampleResponse>
{
    public async Task<UpdateExampleResponse> Handle(
        UpdateExampleCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var item = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (item is null)
        {
            throw new UseCaseNotFoundException($"Example item '{command.Id}' was not found.");
        }

        item.Rename(command.Name);
        await repository.UpdateAsync(item, cancellationToken);
        return new UpdateExampleResponse(ExampleMappings.ToDto(item));
    }
}
