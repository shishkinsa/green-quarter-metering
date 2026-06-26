using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample.Validators;

/// <summary>
/// Валидация <see cref="UpdateExampleCommand"/>.
/// </summary>
public sealed class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
{
    public UpdateExampleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);
    }
}
