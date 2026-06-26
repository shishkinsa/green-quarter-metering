using GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById;

/// <summary>
/// Запрос примера сущности по идентификатору.
/// </summary>
public sealed record GetExampleByIdQuery(Guid Id) : IRequest<GetExampleByIdResponse>;
