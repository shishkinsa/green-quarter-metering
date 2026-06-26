using CategoryEntity = GQ.WebApi.Entities.Category;
using GQ.WebApi.UseCases.Handlers.Category.Dto;

namespace GQ.WebApi.UseCases.Handlers.Category.Mappings;

internal static class CategoryMappings
{
    internal static CategoryDto ToDto(CategoryEntity category) =>
        new(category.Id, category.Code, category.Name);
}
