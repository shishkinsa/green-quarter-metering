using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Commands.DeleteBuilding;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Building;

public sealed class DeleteBuildingCommandHandlerTests
{
    [Fact]
    public async Task DeleteBuildingCommandHandler_RemovesBuildingAndApartments()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();
        context.SeedMeterReadingMay2026();

        DeleteBuildingCommandHandler handler = new(
            context.Buildings,
            context.Apartments,
            context.MeterReadings);

        await handler.Handle(new DeleteBuildingCommand(TestIds.BuildingId), CancellationToken.None);

        Assert.Empty(context.Buildings.Items);
        Assert.Empty(context.Apartments.Items);
        Assert.Empty(context.MeterReadings.Items);
    }

    [Fact]
    public async Task DeleteBuildingCommandHandler_WhenNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
        DeleteBuildingCommandHandler handler = new(
            context.Buildings,
            context.Apartments,
            context.MeterReadings);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(new DeleteBuildingCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
