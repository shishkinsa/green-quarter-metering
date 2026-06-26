using GQ.WebApi.UseCases.Handlers.Building.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;

/// <summary>Команда назначения или обновления владельца квартиры.</summary>
public sealed record UpsertApartmentOwnerCommand(Guid ApartmentId, string FullName, string? Phone)
    : IRequest<UpsertApartmentOwnerResponse>;

/// <summary>Ответ сценария upsert владельца.</summary>
public sealed record UpsertApartmentOwnerResponse(OwnerDto Item);
