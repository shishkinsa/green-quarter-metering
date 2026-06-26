using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Apartment.Queries.ListApartmentsWithOwners;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Apartment;

public sealed class ListApartmentsWithOwnersQueryHandlerTests
{
    [Fact]
    public async Task ListApartmentsWithOwnersQueryHandler_ReturnsApartments()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();

        ListApartmentsWithOwnersQueryHandler handler = new(context.Db, context.ApartmentQueries);
        ListApartmentsWithOwnersResponse response = await handler.Handle(
            new ListApartmentsWithOwnersQuery(TestIds.BuildingId),
            CancellationToken.None);

        Assert.Equal(2, response.Items.Count);
        Assert.Equal("Иванов Иван Иванович", response.Items[0].OwnerFullName);
    }

    [Fact]
    public async Task ListApartmentsWithOwnersQueryHandler_WhenBuildingNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        ListApartmentsWithOwnersQueryHandler handler = new(context.Db, context.ApartmentQueries);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(new ListApartmentsWithOwnersQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
