using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.UseCases.Handlers.Building.Queries.ListBuildings;

/// <summary>Возвращает список всех домов справочника.</summary>
public sealed class ListBuildingsQueryHandler(IDbContext db)
    : IRequestHandler<ListBuildingsQuery, ListBuildingsResponse>
{
    public async Task<ListBuildingsResponse> Handle(
        ListBuildingsQuery request,
        CancellationToken cancellationToken)
    {
        List<Entities.Building> items = await db.Buildings
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
        return new ListBuildingsResponse([.. items.Select(DirectoryMappings.ToDto)]);
    }
}
