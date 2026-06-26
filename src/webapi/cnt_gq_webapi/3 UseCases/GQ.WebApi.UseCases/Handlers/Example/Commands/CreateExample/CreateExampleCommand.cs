using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample;

/// <summary>
/// Команда создания примера сущности.
/// </summary>
public sealed record CreateExampleCommand(string Name) : IRequest<CreateExampleResponse>;
