using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.DeleteExample;

/// <summary>
/// Команда удаления примера сущности.
/// </summary>
public sealed record DeleteExampleCommand(Guid Id) : IRequest;
