using FluentValidation;

using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Dto;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ApartmentEntity = GQ.WebApi.Entities.Apartment;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;

/// <summary>
/// Создаёт квартиру в указанном доме.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
/// <exception cref="UseCaseConflictException">Номер квартиры уже занят в этом доме.</exception>
public sealed class CreateApartmentCommandHandler(
    IDbContext db,
    IValidator<CreateApartmentCommand> validator)
    : IRequestHandler<CreateApartmentCommand, CreateApartmentResponse>
{
    public async Task<CreateApartmentResponse> Handle(
        CreateApartmentCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        _ = await db.Buildings.FirstOrDefaultAsync(x => x.Id == command.BuildingId, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Building '{command.BuildingId}' was not found.");

        string normalizedNumber = command.Number.Trim();
        bool numberExists = await db.Apartments.AnyAsync(
            x => x.BuildingId == command.BuildingId && x.Number == normalizedNumber,
            cancellationToken);

        if(numberExists)
        {
            throw new UseCaseConflictException(
                $"Apartment number '{command.Number}' already exists in building '{command.BuildingId}'.");
        }

        ApartmentEntity apartment = ApartmentEntity.Create(command.BuildingId, command.Number, command.Floor);
        db.Apartments.Add(apartment);
        await db.SaveChangesAsync(cancellationToken);

        var item = new ApartmentWithOwnerDto(
            apartment.Id,
            apartment.BuildingId,
            apartment.Number,
            apartment.Floor,
            null,
            null,
            null,
            null,
            null,
            false);

        return new CreateApartmentResponse(item);
    }
}
