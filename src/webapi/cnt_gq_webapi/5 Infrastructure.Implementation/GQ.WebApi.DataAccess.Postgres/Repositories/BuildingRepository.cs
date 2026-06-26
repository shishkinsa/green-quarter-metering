using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.DataAccess.Postgres.Repositories;

/// <summary>
/// Реализация <see cref="IBuildingRepository"/> на EF Core и PostgreSQL.
/// </summary>
public sealed class BuildingRepository(AppDbContext dbContext): IBuildingRepository
{
    public async Task<IReadOnlyList<Building>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Buildings
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Building?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Buildings.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Building building, CancellationToken cancellationToken = default)
    {
        dbContext.Buildings.Add(building);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Building building, CancellationToken cancellationToken = default)
    {
        dbContext.Buildings.Update(building);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Building building, CancellationToken cancellationToken = default)
    {
        dbContext.Buildings.Remove(building);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
