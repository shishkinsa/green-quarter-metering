using GQ.WebApi.Entities;
using GQ.WebApi.UseCases.Handlers.Example.Dto;

namespace GQ.WebApi.UseCases.Handlers.Example.Mappings;

internal static class ExampleMappings
{
    internal static ExampleItemDto ToDto(ExampleItem item) =>
        new(item.Id, item.Name, item.CreatedAt);
}
