using GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories;

/// <summary>
/// Запрос списка категорий.
/// </summary>
public sealed record ListCategoriesQuery : IRequest<ListCategoriesResponse>;
