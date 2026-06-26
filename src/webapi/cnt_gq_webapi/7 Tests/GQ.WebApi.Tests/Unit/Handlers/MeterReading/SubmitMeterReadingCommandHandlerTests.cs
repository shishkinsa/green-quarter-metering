using FluentValidation;

using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading;
using GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Unit.Handlers.MeterReading;

public sealed class SubmitMeterReadingCommandHandlerTests
{
    [Fact]
    public async Task SubmitMeterReadingCommandHandler_CreatesReading()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);
        SubmitMeterReadingResponse response = await handler.Handle(
            new SubmitMeterReadingCommand(TestIds.Apartment2Id, 2026, 6, 500m),
            CancellationToken.None);

        Assert.True(response.Created);
        Assert.Equal(500m, response.Item.Value);
        Assert.Equal(1, await context.Db.MeterReadings.CountAsync());
    }

    [Fact]
    public async Task SubmitMeterReadingCommandHandler_UpdatesSameMonth()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();
        await context.SeedMeterReadingMay2026Async();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);
        SubmitMeterReadingResponse response = await handler.Handle(
            new SubmitMeterReadingCommand(TestIds.Apartment1Id, 2026, 5, 1050m),
            CancellationToken.None);

        Assert.False(response.Created);
        Assert.Equal(1050m, response.Item.Value);
    }

    [Fact]
    public async Task SubmitMeterReadingCommandHandler_WhenLessThanPrevious_ThrowsValidation()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();
        await context.SeedMeterReadingMay2026Async();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new SubmitMeterReadingCommand(TestIds.Apartment1Id, 2026, 6, 999m),
                CancellationToken.None));
    }

    [Fact]
    public async Task SubmitMeterReadingCommandHandler_WhenApartmentNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        SubmitMeterReadingCommandHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new SubmitMeterReadingCommand(Guid.NewGuid(), 2026, 6, 100m),
                CancellationToken.None));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public async Task SubmitMeterReadingCommandHandler_WhenInvalidMonth_ThrowsValidation(int periodMonth)
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new SubmitMeterReadingCommand(TestIds.Apartment2Id, 2026, periodMonth, 100m),
                CancellationToken.None));
    }

    private static SubmitMeterReadingCommandHandler CreateHandler(HandlerTestContext context)
    {
        return new SubmitMeterReadingCommandHandler(
            context.Db,
            context.MeterReadingQueries,
            new SubmitMeterReadingCommandValidator());
    }
}
