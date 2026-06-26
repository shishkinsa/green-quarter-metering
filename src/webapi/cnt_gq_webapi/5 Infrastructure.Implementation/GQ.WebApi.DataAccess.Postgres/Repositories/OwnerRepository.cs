using Microsoft.EntityFrameworkCore;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;

namespace GQ.WebApi.DataAccess.Postgres.Repositories;

/// <summary>
/// Реализация <see cref="IOwnerRepository"/> на EF Core и PostgreSQL.
/// </summary>
public sealed class OwnerRepository(AppDbContext dbContext) : IOwnerRepository
{
    public Task<Owner?> GetByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken = default)
    {
        return dbContext.Owners.FirstOrDefaultAsync(x => x.ApartmentId == apartmentId, cancellationToken);
    }

    public async Task AddAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        dbContext.Owners.Add(owner);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        dbContext.Owners.Update(owner);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
