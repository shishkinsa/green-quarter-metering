using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Queries.ListApartmentMeterReadings;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.MeterReading;

public sealed class ListApartmentMeterReadingsQueryHandlerTests
{
    [Fact]
    public async Task ListApartmentMeterReadingsQueryHandler_ReturnsHistory()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();
        await context.SeedMeterReadingMay2026Async();

        ListApartmentMeterReadingsQueryHandler handler = new(context.Db);
        ListApartmentMeterReadingsResponse response = await handler.Handle(
            new ListApartmentMeterReadingsQuery(TestIds.Apartment1Id),
            CancellationToken.None);

        Assert.Single(response.Items);
        Assert.Equal(2026, response.Items[0].PeriodYear);
        Assert.Equal(5, response.Items[0].PeriodMonth);
        Assert.Equal(1000m, response.Items[0].Value);
        Assert.NotEqual(default, response.Items[0].SubmittedAt);
    }

    [Fact]
    public async Task ListApartmentMeterReadingsQueryHandler_WhenApartmentNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        ListApartmentMeterReadingsQueryHandler handler = new(context.Db);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(new ListApartmentMeterReadingsQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
