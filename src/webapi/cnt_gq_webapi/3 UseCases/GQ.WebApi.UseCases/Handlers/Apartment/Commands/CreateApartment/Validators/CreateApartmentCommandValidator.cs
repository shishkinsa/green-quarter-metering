using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment.Validators;

/// <summary>Правила валидации команды создания квартиры.</summary>
public sealed class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
{
    public CreateApartmentCommandValidator()
    {
        RuleFor(x => x.BuildingId).NotEmpty();
        RuleFor(x => x.Number).NotEmpty().MaximumLength(32);
    }
}
