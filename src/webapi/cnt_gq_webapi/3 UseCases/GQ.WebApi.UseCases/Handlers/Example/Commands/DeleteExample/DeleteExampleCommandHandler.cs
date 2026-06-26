using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.DeleteExample;

/// <summary>
/// Обработчик команды <see cref="DeleteExampleCommand"/>.
/// </summary>
public sealed class DeleteExampleCommandHandler(IExampleItemRepository repository)
    : IRequestHandler<DeleteExampleCommand>
{
    public async Task Handle(DeleteExampleCommand command, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (item is null)
        {
            throw new UseCaseNotFoundException($"Example item '{command.Id}' was not found.");
        }

        await repository.DeleteAsync(command.Id, cancellationToken);
    }
}
