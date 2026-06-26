using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;

public sealed record CreateApartmentCommand(Guid BuildingId, string Number, int? Floor)
    : IRequest<CreateApartmentResponse>;

public sealed record CreateApartmentResponse(ApartmentWithOwnerDto Item);
