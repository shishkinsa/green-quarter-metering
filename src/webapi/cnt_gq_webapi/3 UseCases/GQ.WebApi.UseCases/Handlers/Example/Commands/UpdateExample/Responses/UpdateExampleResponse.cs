using GQ.WebApi.UseCases.Handlers.Example.Dto;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample.Responses;

/// <summary>
/// Ответ команды обновления примера.
/// </summary>
public sealed record UpdateExampleResponse(ExampleItemDto Item);
