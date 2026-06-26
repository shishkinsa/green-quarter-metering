using System.Net;
using System.Net.Http.Json;

namespace GQ.WebApi.Tests.Integration;

[Collection("api")]
public sealed class BuildingsEndpointTests(WebApiFactory factory): IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ListBuildings_ReturnsSeededBuilding()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/v1/buildings");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        ListBuildingsPayload? payload = await response.Content.ReadFromJsonAsync<ListBuildingsPayload>();
        Assert.NotNull(payload);
        Assert.Contains(payload!.Items, x => x.Name == "Корпус 1");
    }

    [Fact]
    public async Task ListApartmentsWithOwners_ReturnsSeededData()
    {
        var buildingId = Guid.Parse("b0000001-0000-0000-0000-000000000001");
        HttpResponseMessage response = await _client.GetAsync($"/api/v1/buildings/{buildingId}/apartments");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        ListApartmentsPayload? payload = await response.Content.ReadFromJsonAsync<ListApartmentsPayload>();
        Assert.NotNull(payload);
        Assert.Equal(2, payload!.Items.Count);
        Assert.Contains(payload.Items, x => x.Number == "12" && x.OwnerFullName == "Иванов Иван Иванович");
        Assert.Contains(payload.Items, x => x.Number == "15" && x.OwnerFullName is null);
    }

    [Fact]
    public async Task CreateBuilding_AndApartment_Works()
    {
        HttpResponseMessage createBuilding = await _client.PostAsJsonAsync(
            "/api/v1/buildings",
            new { name = "Корпус 2", address = "ул. Садовая, 2" });
        Assert.Equal(HttpStatusCode.Created, createBuilding.StatusCode);
        CreateBuildingPayload? building = await createBuilding.Content.ReadFromJsonAsync<CreateBuildingPayload>();
        Assert.NotNull(building);

        HttpResponseMessage createApartment = await _client.PostAsJsonAsync(
            $"/api/v1/buildings/{building!.Item.Id}/apartments",
            new { number = "1", floor = 1 });
        Assert.Equal(HttpStatusCode.Created, createApartment.StatusCode);
    }

    [Fact]
    public async Task ListApartments_WhenBuildingNotFound_Returns404()
    {
        HttpResponseMessage response = await _client.GetAsync($"/api/v1/buildings/{Guid.NewGuid()}/apartments");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private sealed record ListBuildingsPayload(IReadOnlyList<BuildingPayload> Items);

    private sealed record BuildingPayload(Guid Id, string Name, string? Address);

    private sealed record CreateBuildingPayload(BuildingPayload Item);

    private sealed record ListApartmentsPayload(IReadOnlyList<ApartmentPayload> Items);

    private sealed record ApartmentPayload(
        Guid Id,
        Guid BuildingId,
        string Number,
        int? Floor,
        Guid? OwnerId,
        string? OwnerFullName,
        string? OwnerPhone);
}
