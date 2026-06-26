using FluentValidation;

using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Dto;
using GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListBuildingMeterReadings;
using GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListBuildingMeterReadings.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.MeterReading;

public sealed class ListBuildingMeterReadingsQueryHandlerTests
{
    [Fact]
    public async Task ListBuildingMeterReadingsQueryHandler_ReturnsStatus()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();
        context.SeedMeterReadingMay2026();

        ListBuildingMeterReadingsQueryHandler handler = CreateHandler(context);
        ListBuildingMeterReadingsResponse response = await handler.Handle(
            new ListBuildingMeterReadingsQuery(TestIds.BuildingId, 2026, 5),
            CancellationToken.None);

        Assert.Equal(2, response.Items.Count);

        BuildingMeterReadingStatusDto submitted = response.Items.Single(x => x.ApartmentNumber == "12");
        Assert.True(submitted.Submitted);
        Assert.Equal(1000m, submitted.ReadingValue);

        BuildingMeterReadingStatusDto notSubmitted = response.Items.Single(x => x.ApartmentNumber == "15");
        Assert.False(notSubmitted.Submitted);
        Assert.Null(notSubmitted.ReadingValue);
    }

    [Fact]
    public async Task ListBuildingMeterReadingsQueryHandler_WhenBuildingNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
        ListBuildingMeterReadingsQueryHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new ListBuildingMeterReadingsQuery(Guid.NewGuid(), 2026, 6),
                CancellationToken.None));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public async Task ListBuildingMeterReadingsQueryHandler_WhenInvalidMonth_ThrowsValidation(int periodMonth)
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        ListBuildingMeterReadingsQueryHandler handler = CreateHandler(context);

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(
                new ListBuildingMeterReadingsQuery(TestIds.BuildingId, 2026, periodMonth),
                CancellationToken.None));
    }

    private static ListBuildingMeterReadingsQueryHandler CreateHandler(HandlerTestContext context)
    {
        return new ListBuildingMeterReadingsQueryHandler(
            context.Buildings,
            context.MeterReadings,
            new ListBuildingMeterReadingsQueryValidator());
    }
}
