using GQ.WebApi.UseCases.Handlers.Building.Dto;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;

public sealed record UpsertApartmentOwnerCommand(Guid ApartmentId, string FullName, string? Phone)
    : IRequest<UpsertApartmentOwnerResponse>;

public sealed record UpsertApartmentOwnerResponse(OwnerDto Item);
