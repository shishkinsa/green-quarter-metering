using Microsoft.EntityFrameworkCore;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.DataAccess.Postgres.Repositories;

/// <summary>
/// Реализация репозитория <see cref="ExampleItem"/> через EF Core.
/// </summary>
public sealed class ExampleItemRepository(AppDbContext dbContext) : IExampleItemRepository
{
    public async Task AddAsync(ExampleItem item, CancellationToken cancellationToken = default)
    {
        dbContext.ExampleItems.Add(item);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<ExampleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.ExampleItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExampleItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ExampleItems
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(ExampleItem item, CancellationToken cancellationToken = default)
    {
        dbContext.ExampleItems.Update(item);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await dbContext.ExampleItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (item is null)
        {
            return;
        }

        dbContext.ExampleItems.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
