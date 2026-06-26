using GQ.WebApi.Entities;

namespace GQ.WebApi.UseCases.Handlers.Category.Dto;

/// <summary>
/// DTO справочной категории.
/// </summary>
public sealed record CategoryDto(Guid Id, string Code, string Name);
