using GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples;

/// <summary>
/// Запрос списка примеров сущностей.
/// </summary>
public sealed record ListExamplesQuery : IRequest<ListExamplesResponse>;
