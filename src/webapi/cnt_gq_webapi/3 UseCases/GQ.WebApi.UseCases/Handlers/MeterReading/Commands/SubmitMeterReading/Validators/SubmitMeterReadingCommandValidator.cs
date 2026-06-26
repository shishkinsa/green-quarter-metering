using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading.Validators;

/// <summary>Правила валидации команды передачи показания.</summary>
public sealed class SubmitMeterReadingCommandValidator: AbstractValidator<SubmitMeterReadingCommand>
{
    public SubmitMeterReadingCommandValidator()
    {
        RuleFor(x => x.ApartmentId).NotEmpty();
        RuleFor(x => x.PeriodYear).InclusiveBetween(2000, 2100);
        // freelancer: месяцы 0–11 как в JavaScript Date.getMonth()
        RuleFor(x => x.PeriodMonth).InclusiveBetween(0, 11);
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
    }
}
