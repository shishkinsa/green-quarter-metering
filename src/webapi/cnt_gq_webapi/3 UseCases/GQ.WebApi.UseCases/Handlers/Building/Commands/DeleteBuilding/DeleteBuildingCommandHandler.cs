using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.DeleteBuilding;

/// <summary>
/// Удаляет дом вместе с квартирами, владельцами и показаниями.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
public sealed class DeleteBuildingCommandHandler(IDbContext db)
    : IRequestHandler<DeleteBuildingCommand>
{
    public async Task Handle(DeleteBuildingCommand command, CancellationToken cancellationToken)
    {
        Entities.Building building = await db.Buildings.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Building '{command.Id}' was not found.");

        List<Entities.Apartment> apartments = await db.Apartments
            .Where(x => x.BuildingId == command.Id)
            .ToListAsync(cancellationToken);

        if(apartments.Count > 0)
        {
            HashSet<Guid> apartmentIds = [.. apartments.Select(x => x.Id)];
            List<Entities.MeterReading> readings = await db.MeterReadings
                .Where(x => apartmentIds.Contains(x.ApartmentId))
                .ToListAsync(cancellationToken);

            db.MeterReadings.RemoveRange(readings);
            db.Apartments.RemoveRange(apartments);
        }

        db.Buildings.Remove(building);
        await db.SaveChangesAsync(cancellationToken);
    }
}
