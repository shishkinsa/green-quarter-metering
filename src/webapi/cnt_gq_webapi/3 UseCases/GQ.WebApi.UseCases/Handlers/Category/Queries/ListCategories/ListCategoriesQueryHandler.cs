using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Category.Mappings;
using GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories;

/// <summary>
/// Обработчик <see cref="ListCategoriesQuery"/>.
/// </summary>
public sealed class ListCategoriesQueryHandler(ICategoryRepository repository)
    : IRequestHandler<ListCategoriesQuery, ListCategoriesResponse>
{
    public async Task<ListCategoriesResponse> Handle(
        ListCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var items = await repository.ListAsync(cancellationToken);
        var dtos = items.Select(CategoryMappings.ToDto).ToList();
        return new ListCategoriesResponse(dtos);
    }
}
