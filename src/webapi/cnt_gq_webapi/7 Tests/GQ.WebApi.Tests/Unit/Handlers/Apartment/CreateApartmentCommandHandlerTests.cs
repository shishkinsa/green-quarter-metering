using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;
using GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment.Validators;

using GQ.WebApi.Tests.Unit.TestDoubles;

namespace GQ.WebApi.Tests.Unit.Handlers.Apartment;

public sealed class CreateApartmentCommandHandlerTests
{
    [Fact]
    public async Task CreateApartmentCommandHandler_CreatesApartment()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        CreateApartmentCommandHandler handler = new(
            context.Buildings,
            context.Apartments,
            new CreateApartmentCommandValidator());

        CreateApartmentResponse response = await handler.Handle(
            new CreateApartmentCommand(TestIds.BuildingId, "20", 4),
            CancellationToken.None);

        Assert.Equal("20", response.Item.Number);
        Assert.Equal(3, context.Apartments.Items.Count);
    }

    [Fact]
    public async Task CreateApartmentCommandHandler_WhenBuildingNotFound_ThrowsNotFound()
    {
        HandlerTestContext context = new();
        CreateApartmentCommandHandler handler = new(
            context.Buildings,
            context.Apartments,
            new CreateApartmentCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(
            () => handler.Handle(
                new CreateApartmentCommand(Guid.NewGuid(), "1", 1),
                CancellationToken.None));
    }

    [Fact]
    public async Task CreateApartmentCommandHandler_WhenDuplicateNumber_ThrowsConflict()
    {
        HandlerTestContext context = new();
        context.SeedDirectory();

        CreateApartmentCommandHandler handler = new(
            context.Buildings,
            context.Apartments,
            new CreateApartmentCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(
            () => handler.Handle(
                new CreateApartmentCommand(TestIds.BuildingId, "12", 3),
                CancellationToken.None));
    }
}
