using GQ.WebApi.UseCases.Handlers.Building.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.UpdateApartment;

/// <summary>Команда обновления квартиры.</summary>
public sealed record UpdateApartmentCommand(
    Guid Id,
    string Number,
    int? Floor,
    DateOnly? MeterVerificationDate)
    : IRequest<UpdateApartmentResponse>;

/// <summary>Ответ сценария обновления квартиры.</summary>
public sealed record UpdateApartmentResponse(ApartmentWithOwnerDto Item);
