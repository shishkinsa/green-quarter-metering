using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Commands.DeleteBuilding;

using GQ.WebApi.Tests.Unit.TestDoubles;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Unit.Handlers.Building;

public sealed class DeleteBuildingCommandHandlerTests
{
    [Fact]
    public async Task DeleteBuildingCommandHandler_RemovesBuildingAndApartments()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();
        await context.SeedMeterReadingMay2026Async();

        DeleteBuildingCommandHandler handler = new(context.Db);

        await handler.Handle(new DeleteBuildingCommand(TestIds.BuildingId), CancellationToken.None);

        Assert.Equal(0, await context.Db.Buildings.CountAsync());
        Assert.Equal(0, await context.Db.Apartments.CountAsync());
        Assert.Equal(0, await context.Db.MeterReadings.CountAsync());
    }

    [Fact]
    public async Task DeleteBuildingCommandHandler_WhenNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        DeleteBuildingCommandHandler handler = new(context.Db);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(new DeleteBuildingCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
