using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample;
using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Validators;

namespace GQ.WebApi.Tests.Validators;

public sealed class CreateExampleCommandValidatorTests
{
    private readonly CreateExampleCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenNameIsEmpty_ReturnsError(string name)
    {
        var result = _validator.Validate(new CreateExampleCommand(name));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateExampleCommand.Name));
    }

    [Fact]
    public void Validate_WhenNameExceedsMaxLength_ReturnsError()
    {
        var result = _validator.Validate(new CreateExampleCommand(new string('a', 257)));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateExampleCommand.Name));
    }

    [Fact]
    public void Validate_WhenNameIsValid_ReturnsSuccess()
    {
        var result = _validator.Validate(new CreateExampleCommand("Valid name"));

        Assert.True(result.IsValid);
    }
}
