namespace GQ.WebApi.UseCases.Handlers.MeterReading.Dto;

/// <summary>DTO показания электросчётчика.</summary>
public sealed record MeterReadingDto(
    Guid Id,
    Guid ApartmentId,
    int PeriodYear,
    int PeriodMonth,
    decimal Value);

/// <summary>DTO статуса сдачи показания по квартире.</summary>
public sealed record BuildingMeterReadingStatusDto(
    Guid ApartmentId,
    string ApartmentNumber,
    string? OwnerFullName,
    decimal? ReadingValue,
    bool Submitted);
