using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListBuildingMeterReadings.Validators;

/// <summary>Правила валидации запроса сводки показаний по дому.</summary>
public sealed class ListBuildingMeterReadingsQueryValidator: AbstractValidator<ListBuildingMeterReadingsQuery>
{
    public ListBuildingMeterReadingsQueryValidator()
    {
        RuleFor(x => x.BuildingId).NotEmpty();
        RuleFor(x => x.PeriodYear).InclusiveBetween(2000, 2100);
        RuleFor(x => x.PeriodMonth).InclusiveBetween(1, 12);
    }
}
