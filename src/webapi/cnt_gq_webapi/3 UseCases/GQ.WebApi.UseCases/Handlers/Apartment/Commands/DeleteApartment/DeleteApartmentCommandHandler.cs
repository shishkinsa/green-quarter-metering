using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.DeleteApartment;

/// <summary>
/// Удаляет квартиру вместе с показаниями; владелец удаляется каскадом.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
public sealed class DeleteApartmentCommandHandler(
    IApartmentRepository apartmentRepository,
    IMeterReadingRepository meterReadingRepository)
    : IRequestHandler<DeleteApartmentCommand>
{
    public async Task Handle(DeleteApartmentCommand command, CancellationToken cancellationToken)
    {
        Entities.Apartment apartment = await apartmentRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new UseCaseNotFoundException($"Apartment '{command.Id}' was not found.");

        await meterReadingRepository.DeleteByApartmentAsync(command.Id, cancellationToken);
        await apartmentRepository.DeleteAsync(apartment, cancellationToken);
    }
}
