using GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample;

/// <summary>
/// Команда обновления примера сущности.
/// </summary>
public sealed record UpdateExampleCommand(Guid Id, string Name) : IRequest<UpdateExampleResponse>;
