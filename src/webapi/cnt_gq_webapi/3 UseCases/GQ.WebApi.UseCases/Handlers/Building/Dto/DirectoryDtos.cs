namespace GQ.WebApi.UseCases.Handlers.Building.Dto;

public sealed record BuildingDto(Guid Id, string Name, string? Address);

public sealed record ApartmentWithOwnerDto(
    Guid Id,
    Guid BuildingId,
    string Number,
    int? Floor,
    Guid? OwnerId,
    string? OwnerFullName,
    string? OwnerPhone);

public sealed record OwnerDto(Guid Id, Guid ApartmentId, string FullName, string? Phone);
