using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Validators;

/// <summary>
/// Валидация <see cref="CreateExampleCommand"/>.
/// </summary>
public sealed class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);
    }
}
