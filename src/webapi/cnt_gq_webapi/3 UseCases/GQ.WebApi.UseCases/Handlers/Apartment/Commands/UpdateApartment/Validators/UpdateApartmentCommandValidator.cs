using FluentValidation;

using GQ.WebApi.UseCases.Validation;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.UpdateApartment.Validators;

/// <summary>Правила валидации команды обновления квартиры.</summary>
public sealed class UpdateApartmentCommandValidator: AbstractValidator<UpdateApartmentCommand>
{
    public UpdateApartmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Number).NotEmpty().MaximumLength(32);
        RuleFor(x => x.MeterVerificationDate)
            .Must(MeterVerificationDateRules.NotInFuture)
            .WithMessage("Meter verification date cannot be in the future.")
            .When(x => x.MeterVerificationDate.HasValue);
    }
}
