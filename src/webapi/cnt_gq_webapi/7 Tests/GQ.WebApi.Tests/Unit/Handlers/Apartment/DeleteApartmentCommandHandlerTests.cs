using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.DeleteApartment;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Apartment;

public sealed class DeleteApartmentCommandHandlerTests
{
    [Fact]
    public async Task DeleteApartmentCommandHandler_RemovesApartmentAndReadings()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();
        context.SeedMeterReadingMay2026();

        DeleteApartmentCommandHandler handler = new(context.Apartments, context.MeterReadings);

        await handler.Handle(new DeleteApartmentCommand(TestIds.Apartment1Id), CancellationToken.None);

        Assert.Single(context.Apartments.Items);
        Assert.Empty(context.MeterReadings.Items);
    }

    [Fact]
    public async Task DeleteApartmentCommandHandler_WhenNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
        DeleteApartmentCommandHandler handler = new(context.Apartments, context.MeterReadings);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(new DeleteApartmentCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
