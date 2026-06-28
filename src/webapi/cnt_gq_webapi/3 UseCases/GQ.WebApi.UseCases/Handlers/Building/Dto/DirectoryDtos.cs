namespace GQ.WebApi.UseCases.Handlers.Building.Dto;

/// <summary>Дом в ответе API справочника.</summary>
public sealed record BuildingDto(Guid Id, string Name, string? Address);

/// <summary>Квартира с опциональными данными владельца в ответе API.</summary>
public sealed record ApartmentWithOwnerDto(
    Guid Id,
    Guid BuildingId,
    string Number,
    int? Floor,
    DateOnly? MeterVerificationDate,
    Guid? OwnerId,
    string? OwnerFullName,
    string? OwnerPhone,
    DateTimeOffset? LastReadingSubmittedAt,
    decimal? LastReadingValue,
    bool CurrentPeriodSubmitted);

/// <summary>Владелец квартиры в ответе API.</summary>
public sealed record OwnerDto(Guid Id, Guid ApartmentId, string FullName, string? Phone);
