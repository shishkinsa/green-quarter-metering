using FluentValidation;
using ApartmentEntity = GQ.WebApi.Entities.Apartment;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;

/// <summary>
/// Создаёт квартиру в указанном доме.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
/// <exception cref="UseCaseConflictException">Номер квартиры уже занят в этом доме.</exception>
public sealed class CreateApartmentCommandHandler(
    IBuildingRepository buildingRepository,
    IApartmentRepository apartmentRepository,
    IValidator<CreateApartmentCommand> validator)
    : IRequestHandler<CreateApartmentCommand, CreateApartmentResponse>
{
    public async Task<CreateApartmentResponse> Handle(
        CreateApartmentCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var building = await buildingRepository.GetByIdAsync(command.BuildingId, cancellationToken);
        if (building is null)
        {
            throw new UseCaseNotFoundException($"Building '{command.BuildingId}' was not found.");
        }

        if (await apartmentRepository.ExistsByBuildingAndNumberAsync(
                command.BuildingId,
                command.Number,
                null,
                cancellationToken))
        {
            throw new UseCaseConflictException(
                $"Apartment number '{command.Number}' already exists in building '{command.BuildingId}'.");
        }

        var apartment = ApartmentEntity.Create(command.BuildingId, command.Number, command.Floor);
        await apartmentRepository.AddAsync(apartment, cancellationToken);

        var item = new ApartmentWithOwnerDto(
            apartment.Id,
            apartment.BuildingId,
            apartment.Number,
            apartment.Floor,
            null,
            null,
            null);

        return new CreateApartmentResponse(item);
    }
}
