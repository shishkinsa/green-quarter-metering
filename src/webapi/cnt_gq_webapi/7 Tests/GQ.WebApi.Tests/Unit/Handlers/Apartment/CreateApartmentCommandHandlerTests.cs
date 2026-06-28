using FluentValidation;

using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment.Validators;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.UpdateApartment;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.UpdateApartment.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Unit.Handlers.Apartment;

public sealed class CreateApartmentCommandHandlerTests
{
    [Fact]
    public async Task CreateApartmentCommandHandler_CreatesApartment()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        CreateApartmentCommandHandler handler = new(
            context.Db,
            new CreateApartmentCommandValidator());

        CreateApartmentResponse response = await handler.Handle(
            new CreateApartmentCommand(TestIds.BuildingId, "20", 4, null),
            CancellationToken.None);

        Assert.Equal("20", response.Item.Number);
        Assert.Equal(3, await context.Db.Apartments.CountAsync());
    }

    [Fact]
    public async Task CreateApartmentCommandHandler_CreatesApartmentWithMeterVerificationDate()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        DateOnly verificationDate = new(2024, 6, 15);
        CreateApartmentCommandHandler handler = new(
            context.Db,
            new CreateApartmentCommandValidator());

        CreateApartmentResponse response = await handler.Handle(
            new CreateApartmentCommand(TestIds.BuildingId, "20", 4, verificationDate),
            CancellationToken.None);

        Assert.Equal(verificationDate, response.Item.MeterVerificationDate);
    }

    [Fact]
    public async Task CreateApartmentCommandHandler_WhenFutureVerificationDate_ThrowsValidation()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        CreateApartmentCommandHandler handler = new(
            context.Db,
            new CreateApartmentCommandValidator());

        DateOnly futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new CreateApartmentCommand(TestIds.BuildingId, "20", 4, futureDate),
                CancellationToken.None));
    }

    [Fact]
    public async Task CreateApartmentCommandHandler_WhenBuildingNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        CreateApartmentCommandHandler handler = new(
            context.Db,
            new CreateApartmentCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new CreateApartmentCommand(Guid.NewGuid(), "1", 1, null),
                CancellationToken.None));
    }

    [Fact]
    public async Task CreateApartmentCommandHandler_WhenDuplicateNumber_ThrowsConflict()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        CreateApartmentCommandHandler handler = new(
            context.Db,
            new CreateApartmentCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(
            () => handler.Handle(
                new CreateApartmentCommand(TestIds.BuildingId, "12", 3, null),
                CancellationToken.None));
    }
}

public sealed class UpdateApartmentCommandHandlerTests
{
    [Fact]
    public async Task UpdateApartmentCommandHandler_UpdatesApartment()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        DateOnly verificationDate = new(2025, 1, 10);
        UpdateApartmentCommandHandler handler = new(
            context.Db,
            new UpdateApartmentCommandValidator());

        UpdateApartmentResponse response = await handler.Handle(
            new UpdateApartmentCommand(TestIds.Apartment1Id, "12а", 4, verificationDate),
            CancellationToken.None);

        Assert.Equal("12а", response.Item.Number);
        Assert.Equal(4, response.Item.Floor);
        Assert.Equal(verificationDate, response.Item.MeterVerificationDate);
        Assert.Equal("Иванов Иван Иванович", response.Item.OwnerFullName);
    }

    [Fact]
    public async Task UpdateApartmentCommandHandler_ClearsMeterVerificationDate()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        UpdateApartmentCommandHandler handler = new(
            context.Db,
            new UpdateApartmentCommandValidator());

        await handler.Handle(
            new UpdateApartmentCommand(TestIds.Apartment1Id, "12", 3, new DateOnly(2024, 1, 1)),
            CancellationToken.None);

        UpdateApartmentResponse response = await handler.Handle(
            new UpdateApartmentCommand(TestIds.Apartment1Id, "12", 3, null),
            CancellationToken.None);

        Assert.Null(response.Item.MeterVerificationDate);
    }

    [Fact]
    public async Task UpdateApartmentCommandHandler_WhenFutureVerificationDate_ThrowsValidation()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        UpdateApartmentCommandHandler handler = new(
            context.Db,
            new UpdateApartmentCommandValidator());

        DateOnly futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new UpdateApartmentCommand(TestIds.Apartment1Id, "12", 3, futureDate),
                CancellationToken.None));
    }

    [Fact]
    public async Task UpdateApartmentCommandHandler_WhenApartmentNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        UpdateApartmentCommandHandler handler = new(
            context.Db,
            new UpdateApartmentCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new UpdateApartmentCommand(Guid.NewGuid(), "1", 1, null),
                CancellationToken.None));
    }

    [Fact]
    public async Task UpdateApartmentCommandHandler_WhenDuplicateNumber_ThrowsConflict()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        UpdateApartmentCommandHandler handler = new(
            context.Db,
            new UpdateApartmentCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(
            () => handler.Handle(
                new UpdateApartmentCommand(TestIds.Apartment1Id, "15", 3, null),
                CancellationToken.None));
    }
}
