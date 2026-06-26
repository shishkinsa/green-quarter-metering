using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.DeleteApartment;

/// <summary>
/// Удаляет квартиру вместе с показаниями; владелец удаляется каскадом.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
public sealed class DeleteApartmentCommandHandler(IDbContext db)
    : IRequestHandler<DeleteApartmentCommand>
{
    public async Task Handle(DeleteApartmentCommand command, CancellationToken cancellationToken)
    {
        Entities.Apartment apartment = await db.Apartments.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Apartment '{command.Id}' was not found.");

        List<Entities.MeterReading> readings = await db.MeterReadings
            .Where(x => x.ApartmentId == command.Id)
            .ToListAsync(cancellationToken);

        db.MeterReadings.RemoveRange(readings);
        db.Apartments.Remove(apartment);
        await db.SaveChangesAsync(cancellationToken);
    }
}
