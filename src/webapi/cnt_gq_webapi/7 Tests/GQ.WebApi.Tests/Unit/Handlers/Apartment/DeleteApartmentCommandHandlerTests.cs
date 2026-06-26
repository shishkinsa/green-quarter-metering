using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.DeleteApartment;

using GQ.WebApi.Tests.Unit.TestDoubles;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Unit.Handlers.Apartment;

public sealed class DeleteApartmentCommandHandlerTests
{
    [Fact]
    public async Task DeleteApartmentCommandHandler_RemovesApartmentAndReadings()
    {
        await using HandlerTestContext context = new();
        await context.SeedDirectoryAsync();
        await context.SeedMeterReadingMay2026Async();

        DeleteApartmentCommandHandler handler = new(context.Db);

        await handler.Handle(new DeleteApartmentCommand(TestIds.Apartment1Id), CancellationToken.None);

        Assert.Equal(1, await context.Db.Apartments.CountAsync());
        Assert.Equal(0, await context.Db.MeterReadings.CountAsync());
    }

    [Fact]
    public async Task DeleteApartmentCommandHandler_WhenNotFound_ThrowsNotFound()
    {
        await using HandlerTestContext context = new();
        DeleteApartmentCommandHandler handler = new(context.Db);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(new DeleteApartmentCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
