using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding;
using GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Building;

public sealed class UpdateBuildingCommandHandlerTests
{
    [Fact]
    public async Task UpdateBuildingCommandHandler_UpdatesBuilding()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        UpdateBuildingCommandHandler handler = new(
            context.Buildings,
            new UpdateBuildingCommandValidator());

        UpdateBuildingResponse response = await handler.Handle(
            new UpdateBuildingCommand(TestIds.BuildingId, "Корпус 1 (обновлён)", "ул. Зелёная, 10"),
            CancellationToken.None);

        Assert.Equal("Корпус 1 (обновлён)", response.Item.Name);
        Assert.Equal("ул. Зелёная, 10", response.Item.Address);
    }

    [Fact]
    public async Task UpdateBuildingCommandHandler_WhenBuildingNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
        UpdateBuildingCommandHandler handler = new(
            context.Buildings,
            new UpdateBuildingCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new UpdateBuildingCommand(Guid.NewGuid(), "Корпус", null),
                CancellationToken.None));
    }
}
