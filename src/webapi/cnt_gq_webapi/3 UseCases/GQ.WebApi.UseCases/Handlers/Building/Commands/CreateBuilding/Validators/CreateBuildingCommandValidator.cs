using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding.Validators;

/// <summary>Правила валидации команды создания дома.</summary>
public sealed class CreateBuildingCommandValidator: AbstractValidator<CreateBuildingCommand>
{
    public CreateBuildingCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Address).MaximumLength(512);
    }
}
