using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.DeleteApartment;

/// <summary>Команда удаления квартиры и связанных данных.</summary>
public sealed record DeleteApartmentCommand(Guid Id): IRequest;
