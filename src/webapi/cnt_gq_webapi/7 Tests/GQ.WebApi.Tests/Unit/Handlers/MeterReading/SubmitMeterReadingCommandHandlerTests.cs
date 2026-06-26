using FluentValidation;

using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading;
using GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.MeterReading;

public sealed class SubmitMeterReadingCommandHandlerTests
{
    [Fact]
    public async Task SubmitMeterReadingCommandHandler_CreatesReading()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);
        SubmitMeterReadingResponse response = await handler.Handle(
            new SubmitMeterReadingCommand(TestIds.Apartment2Id, 2026, 6, 500m),
            CancellationToken.None);

        Assert.True(response.Created);
        Assert.Equal(500m, response.Item.Value);
        Assert.Single(context.MeterReadings.Items);
    }

    [Fact]
    public async Task SubmitMeterReadingCommandHandler_UpdatesSameMonth()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();
        context.SeedMeterReadingMay2026();

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
        HandlerTestContext context = new();
        context.SeedDirectory();
        context.SeedMeterReadingMay2026();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new SubmitMeterReadingCommand(TestIds.Apartment1Id, 2026, 6, 999m),
                CancellationToken.None));
    }

    [Fact]
    public async Task SubmitMeterReadingCommandHandler_WhenApartmentNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
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
        HandlerTestContext context = new();
        context.SeedDirectory();

        SubmitMeterReadingCommandHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new SubmitMeterReadingCommand(TestIds.Apartment2Id, 2026, periodMonth, 100m),
                CancellationToken.None));
    }

    private static SubmitMeterReadingCommandHandler CreateHandler(HandlerTestContext context)
    {
        return new SubmitMeterReadingCommandHandler(
            context.Apartments,
            context.MeterReadings,
            new SubmitMeterReadingCommandValidator());
    }
}
