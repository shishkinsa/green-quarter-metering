using FluentValidation;

using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;
using GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Owner;

public sealed class UpsertApartmentOwnerCommandHandlerTests
{
    [Fact]
    public async Task UpsertApartmentOwnerCommandHandler_CreatesOwner()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        UpsertApartmentOwnerCommandHandler handler = new(
            context.Apartments,
            context.Owners,
            new UpsertApartmentOwnerCommandValidator());

        UpsertApartmentOwnerResponse response = await handler.Handle(
            new UpsertApartmentOwnerCommand(
                TestIds.Apartment2Id,
                "Петров Пётр Петрович",
                "+79007654321"),
            CancellationToken.None);

        Assert.Equal("Петров Пётр Петрович", response.Item.FullName);
        Assert.Equal(2, context.Owners.Items.Count);
    }

    [Fact]
    public async Task UpsertApartmentOwnerCommandHandler_UpdatesOwner()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        UpsertApartmentOwnerCommandHandler handler = new(
            context.Apartments,
            context.Owners,
            new UpsertApartmentOwnerCommandValidator());

        UpsertApartmentOwnerResponse response = await handler.Handle(
            new UpsertApartmentOwnerCommand(
                TestIds.Apartment1Id,
                "Иванов Иван Обновлённый",
                "+79001111111"),
            CancellationToken.None);

        Assert.Equal("Иванов Иван Обновлённый", response.Item.FullName);
        Assert.Single(context.Owners.Items);
    }

    [Fact]
    public async Task UpsertApartmentOwnerCommandHandler_WhenApartmentNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
        UpsertApartmentOwnerCommandHandler handler = new(
            context.Apartments,
            context.Owners,
            new UpsertApartmentOwnerCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new UpsertApartmentOwnerCommand(Guid.NewGuid(), "Test", null),
                CancellationToken.None));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpsertApartmentOwnerCommandHandler_WhenInvalidName_ThrowsValidation(string fullName)
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        UpsertApartmentOwnerCommandHandler handler = new(
            context.Apartments,
            context.Owners,
            new UpsertApartmentOwnerCommandValidator());

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new UpsertApartmentOwnerCommand(TestIds.Apartment1Id, fullName, null),
                CancellationToken.None));
    }
}
