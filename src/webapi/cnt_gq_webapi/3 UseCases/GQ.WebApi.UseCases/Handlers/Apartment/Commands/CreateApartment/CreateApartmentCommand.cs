using GQ.WebApi.UseCases.Handlers.Building.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;

/// <summary>Команда создания квартиры в указанном доме.</summary>
public sealed record CreateApartmentCommand(
    Guid BuildingId,
    string Number,
    int? Floor,
    DateOnly? MeterVerificationDate)
    : IRequest<CreateApartmentResponse>;

/// <summary>Ответ сценария создания квартиры.</summary>
public sealed record CreateApartmentResponse(ApartmentWithOwnerDto Item);
