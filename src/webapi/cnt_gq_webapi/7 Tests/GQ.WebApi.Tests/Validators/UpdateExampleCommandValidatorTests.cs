using GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample;
using GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample.Validators;

namespace GQ.WebApi.Tests.Validators;

public sealed class UpdateExampleCommandValidatorTests
{
    private readonly UpdateExampleCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenNameIsEmpty_ReturnsError(string name)
    {
        var result = _validator.Validate(new UpdateExampleCommand(Guid.NewGuid(), name));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateExampleCommand.Name));
    }

    [Fact]
    public void Validate_WhenIdIsEmpty_ReturnsError()
    {
        var result = _validator.Validate(new UpdateExampleCommand(Guid.Empty, "Valid name"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateExampleCommand.Id));
    }

    [Fact]
    public void Validate_WhenCommandIsValid_ReturnsSuccess()
    {
        var result = _validator.Validate(new UpdateExampleCommand(Guid.NewGuid(), "Valid name"));

        Assert.True(result.IsValid);
    }
}
