using FluentValidation;

using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

using OwnerEntity = GQ.WebApi.Entities.Owner;

namespace GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;

/// <summary>
/// Назначает владельца квартиры или обновляет его контактные данные.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
public sealed class UpsertApartmentOwnerCommandHandler(
    IDbContext db,
    IValidator<UpsertApartmentOwnerCommand> validator)
    : IRequestHandler<UpsertApartmentOwnerCommand, UpsertApartmentOwnerResponse>
{
    public async Task<UpsertApartmentOwnerResponse> Handle(
        UpsertApartmentOwnerCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        _ = await db.Apartments.FirstOrDefaultAsync(x => x.Id == command.ApartmentId, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Apartment '{command.ApartmentId}' was not found.");

        OwnerEntity? existing = await db.Owners.FirstOrDefaultAsync(
            x => x.ApartmentId == command.ApartmentId,
            cancellationToken);

        if(existing is null)
        {
            OwnerEntity owner = OwnerEntity.Create(command.ApartmentId, command.FullName, command.Phone);
            db.Owners.Add(owner);
            await db.SaveChangesAsync(cancellationToken);
            return new UpsertApartmentOwnerResponse(DirectoryMappings.ToDto(owner));
        }

        existing.Update(command.FullName, command.Phone);
        await db.SaveChangesAsync(cancellationToken);
        return new UpsertApartmentOwnerResponse(DirectoryMappings.ToDto(existing));
    }
}
