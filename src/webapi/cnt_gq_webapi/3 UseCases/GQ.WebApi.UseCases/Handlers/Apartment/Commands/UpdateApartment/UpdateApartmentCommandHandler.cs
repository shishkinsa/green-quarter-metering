using FluentValidation;

using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ApartmentEntity = GQ.WebApi.Entities.Apartment;
using OwnerEntity = GQ.WebApi.Entities.Owner;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.UpdateApartment;

/// <summary>
/// Обновляет данные существующей квартиры.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
/// <exception cref="UseCaseConflictException">Номер квартиры уже занят в этом доме.</exception>
public sealed class UpdateApartmentCommandHandler(
    IDbContext db,
    IValidator<UpdateApartmentCommand> validator)
    : IRequestHandler<UpdateApartmentCommand, UpdateApartmentResponse>
{
    public async Task<UpdateApartmentResponse> Handle(
        UpdateApartmentCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        ApartmentEntity apartment = await db.Apartments.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Apartment '{command.Id}' was not found.");

        string normalizedNumber = command.Number.Trim();
        bool numberExists = await db.Apartments.AnyAsync(
            x => x.BuildingId == apartment.BuildingId
                 && x.Number == normalizedNumber
                 && x.Id != apartment.Id,
            cancellationToken);

        if(numberExists)
        {
            throw new UseCaseConflictException(
                $"Apartment number '{command.Number}' already exists in building '{apartment.BuildingId}'.");
        }

        apartment.Update(command.Number, command.Floor, command.MeterVerificationDate);
        await db.SaveChangesAsync(cancellationToken);

        OwnerEntity? owner = await db.Owners.FirstOrDefaultAsync(
            x => x.ApartmentId == apartment.Id,
            cancellationToken);

        return new UpdateApartmentResponse(DirectoryMappings.ToApartmentDto(apartment, owner));
    }
}
