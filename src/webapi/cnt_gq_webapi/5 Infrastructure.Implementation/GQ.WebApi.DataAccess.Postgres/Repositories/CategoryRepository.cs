using Microsoft.EntityFrameworkCore;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.DataAccess.Postgres.Repositories;

/// <summary>
/// Read-only репозиторий справочника категорий.
/// </summary>
public sealed class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);
    }
}
