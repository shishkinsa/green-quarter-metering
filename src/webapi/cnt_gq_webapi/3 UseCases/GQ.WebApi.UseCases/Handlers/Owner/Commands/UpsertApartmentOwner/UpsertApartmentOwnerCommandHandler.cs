using FluentValidation;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;
using MediatR;
using OwnerEntity = GQ.WebApi.Entities.Owner;

namespace GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;

/// <summary>
/// Назначает владельца квартиры или обновляет его контактные данные.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
public sealed class UpsertApartmentOwnerCommandHandler(
    IApartmentRepository apartmentRepository,
    IOwnerRepository ownerRepository,
    IValidator<UpsertApartmentOwnerCommand> validator)
    : IRequestHandler<UpsertApartmentOwnerCommand, UpsertApartmentOwnerResponse>
{
    public async Task<UpsertApartmentOwnerResponse> Handle(
        UpsertApartmentOwnerCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var apartment = await apartmentRepository.GetByIdAsync(command.ApartmentId, cancellationToken);
        if (apartment is null)
        {
            throw new UseCaseNotFoundException($"Apartment '{command.ApartmentId}' was not found.");
        }

        var existing = await ownerRepository.GetByApartmentIdAsync(command.ApartmentId, cancellationToken);
        if (existing is null)
        {
            var owner = OwnerEntity.Create(command.ApartmentId, command.FullName, command.Phone);
            await ownerRepository.AddAsync(owner, cancellationToken);
            return new UpsertApartmentOwnerResponse(DirectoryMappings.ToDto(owner));
        }

        existing.Update(command.FullName, command.Phone);
        await ownerRepository.UpdateAsync(existing, cancellationToken);
        return new UpsertApartmentOwnerResponse(DirectoryMappings.ToDto(existing));
    }
}
