using GQ.WebApi.UseCases.Handlers.Example.Dto;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Responses;

/// <summary>
/// Ответ сценария создания примера.
/// </summary>
public sealed record CreateExampleResponse(ExampleItemDto Item);
