using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;
using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Building;

public sealed class CreateBuildingCommandHandlerTests
{
    [Fact]
    public async Task CreateBuildingCommandHandler_CreatesBuilding()
    {
        HandlerTestContext context = new();
        CreateBuildingCommandHandler handler = new(
            context.Buildings,
            new CreateBuildingCommandValidator());

        CreateBuildingResponse response = await handler.Handle(
            new CreateBuildingCommand("Корпус 2", "ул. Новая, 2"),
            CancellationToken.None);

        Assert.Equal("Корпус 2", response.Item.Name);
        Assert.Single(context.Buildings.Items);
    }
}
