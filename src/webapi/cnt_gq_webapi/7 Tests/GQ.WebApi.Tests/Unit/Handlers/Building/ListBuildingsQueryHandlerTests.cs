using GQ.WebApi.UseCases.Handlers.Building.Queries.ListBuildings;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Building;

public sealed class ListBuildingsQueryHandlerTests
{
    [Fact]
    public async Task ListBuildingsQueryHandler_ReturnsBuildings()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        ListBuildingsQueryHandler handler = new(context.Db);
        ListBuildingsResponse response = await handler.Handle(new ListBuildingsQuery(), CancellationToken.None);

        Assert.Single(response.Items);
        Assert.Equal("Корпус 1", response.Items[0].Name);
    }
}
