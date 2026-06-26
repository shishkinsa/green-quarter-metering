using GQ.WebApi.UseCases.Handlers.MeterReading.Dto;

using MediatR;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading;

/// <summary>Команда передачи или обновления показания по квартире.</summary>
public sealed record SubmitMeterReadingCommand(
    Guid ApartmentId,
    int PeriodYear,
    int PeriodMonth,
    decimal Value): IRequest<SubmitMeterReadingResponse>;

/// <summary>Ответ на передачу показания.</summary>
public sealed record SubmitMeterReadingResponse(MeterReadingDto Item, bool Created);
