using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner.Validators;

/// <summary>Правила валидации команды назначения владельца квартиры.</summary>
public sealed class UpsertApartmentOwnerCommandValidator : AbstractValidator<UpsertApartmentOwnerCommand>
{
    public UpsertApartmentOwnerCommandValidator()
    {
        RuleFor(x => x.ApartmentId).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Phone).MaximumLength(32);
    }
}
