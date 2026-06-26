using GQ.WebApi.UseCases.Handlers.Category.Dto;

namespace GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories.Responses;

/// <summary>
/// Ответ со списком категорий.
/// </summary>
public sealed record ListCategoriesResponse(IReadOnlyList<CategoryDto> Items);
