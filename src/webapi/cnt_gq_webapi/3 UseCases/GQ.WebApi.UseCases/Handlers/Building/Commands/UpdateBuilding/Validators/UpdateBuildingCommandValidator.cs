using FluentValidation;

namespace GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding.Validators;

/// <summary>Правила валидации команды обновления дома.</summary>
public sealed class UpdateBuildingCommandValidator : AbstractValidator<UpdateBuildingCommand>
{
    public UpdateBuildingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Address).MaximumLength(512);
    }
}
