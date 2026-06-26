using GQ.WebApi.UseCases.Handlers.Example.Dto;

namespace GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples.Responses;

/// <summary>
/// Ответ со списком примеров.
/// </summary>
public sealed record ListExamplesResponse(IReadOnlyList<ExampleItemDto> Items);
