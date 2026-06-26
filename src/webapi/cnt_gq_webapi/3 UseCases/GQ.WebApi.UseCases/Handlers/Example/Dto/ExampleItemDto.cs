namespace GQ.WebApi.UseCases.Handlers.Example.Dto;

/// <summary>
/// DTO примера сущности для ответов API.
/// </summary>
public sealed record ExampleItemDto(Guid Id, string Name, DateTimeOffset CreatedAt);
