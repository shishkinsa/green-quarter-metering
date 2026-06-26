using GQ.WebApi.UseCases.Handlers.Example.Dto;

namespace GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById.Responses;

/// <summary>
/// Ответ с одним примером сущности.
/// </summary>
public sealed record GetExampleByIdResponse(ExampleItemDto Item);
