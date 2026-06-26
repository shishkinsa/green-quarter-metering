using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Queries.ListBuildings;

/// <summary>Возвращает список всех домов справочника.</summary>
public sealed class ListBuildingsQueryHandler(IBuildingRepository repository)
    : IRequestHandler<ListBuildingsQuery, ListBuildingsResponse>
{
    public async Task<ListBuildingsResponse> Handle(
        ListBuildingsQuery request,
        CancellationToken cancellationToken)
    {
        var items = await repository.ListAsync(cancellationToken);
        return new ListBuildingsResponse(items.Select(DirectoryMappings.ToDto).ToList());
    }
}
