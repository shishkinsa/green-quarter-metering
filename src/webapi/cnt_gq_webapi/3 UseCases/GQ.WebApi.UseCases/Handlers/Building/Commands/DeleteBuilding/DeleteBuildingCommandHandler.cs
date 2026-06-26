using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.DeleteBuilding;

/// <summary>
/// Удаляет дом вместе с квартирами, владельцами и показаниями.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Дом не найден.</exception>
public sealed class DeleteBuildingCommandHandler(
    IBuildingRepository buildingRepository,
    IApartmentRepository apartmentRepository,
    IMeterReadingRepository meterReadingRepository)
    : IRequestHandler<DeleteBuildingCommand>
{
    public async Task Handle(DeleteBuildingCommand command, CancellationToken cancellationToken)
    {
        Entities.Building building = await buildingRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Building '{command.Id}' was not found.");

        IReadOnlyList<Guid> apartmentIds = await apartmentRepository.ListIdsByBuildingAsync(
            command.Id,
            cancellationToken);

        foreach(Guid apartmentId in apartmentIds)
        {
            await meterReadingRepository.DeleteByApartmentAsync(apartmentId, cancellationToken);
            Entities.Apartment apartment = await apartmentRepository.GetByIdAsync(apartmentId, cancellationToken)
                ?? throw new InvalidOperationException($"Apartment '{apartmentId}' was not found.");
            await apartmentRepository.DeleteAsync(apartment, cancellationToken);
        }

        await buildingRepository.DeleteAsync(building, cancellationToken);
    }
}
