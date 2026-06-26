using FluentValidation;

using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;
using GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Unit.Handlers.Owner;

public sealed class UpsertApartmentOwnerCommandHandlerTests
{
    [Fact]
    public async Task UpsertApartmentOwnerCommandHandler_CreatesOwner()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        UpsertApartmentOwnerCommandHandler handler = new(
            context.Db,
            new UpsertApartmentOwnerCommandValidator());

        UpsertApartmentOwnerResponse response = await handler.Handle(
            new UpsertApartmentOwnerCommand(
                TestIds.Apartment2Id,
                "Петров Пётр Петрович",
                "+79007654321"),
            CancellationToken.None);

        Assert.Equal("Петров Пётр Петрович", response.Item.FullName);
        Assert.Equal(2, await context.Db.Owners.CountAsync());
    }

    [Fact]
    public async Task UpsertApartmentOwnerCommandHandler_UpdatesOwner()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        UpsertApartmentOwnerCommandHandler handler = new(
            context.Db,
            new UpsertApartmentOwnerCommandValidator());

        UpsertApartmentOwnerResponse response = await handler.Handle(
            new UpsertApartmentOwnerCommand(
                TestIds.Apartment1Id,
                "Иванов Иван Обновлённый",
                "+79001111111"),
            CancellationToken.None);

        Assert.Equal("Иванов Иван Обновлённый", response.Item.FullName);
        Assert.Equal(1, await context.Db.Owners.CountAsync());
    }

    [Fact]
    public async Task UpsertApartmentOwnerCommandHandler_WhenApartmentNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        UpsertApartmentOwnerCommandHandler handler = new(
            context.Db,
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
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        UpsertApartmentOwnerCommandHandler handler = new(
            context.Db,
            new UpsertApartmentOwnerCommandValidator());

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new UpsertApartmentOwnerCommand(TestIds.Apartment1Id, fullName, null),
                CancellationToken.None));
    }
}
