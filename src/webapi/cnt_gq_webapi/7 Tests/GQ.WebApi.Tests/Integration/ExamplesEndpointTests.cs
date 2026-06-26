using System.Net;
using System.Net.Http.Json;

namespace GQ.WebApi.Tests.Integration;

[Collection("api")]
public sealed class ExamplesEndpointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetHealth_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateAndListExample_Works()
    {
        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/examples",
            new { name = "Integration test" });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var listResponse = await _client.GetAsync("/api/v1/examples");
        listResponse.EnsureSuccessStatusCode();

        var list = await listResponse.Content.ReadFromJsonAsync<ListExamplesPayload>();
        Assert.NotNull(list);
        Assert.Contains(list!.Items, x => x.Name == "Integration test");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateExample_WithInvalidName_ReturnsBadRequest(string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/v1/examples",
            new { name });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetExampleById_WhenExists_ReturnsOk()
    {
        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/examples",
            new { name = "Lookup test" });
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateExamplePayload>();
        Assert.NotNull(created);

        var getResponse = await _client.GetAsync($"/api/v1/examples/{created!.Item.Id}");
        getResponse.EnsureSuccessStatusCode();

        var payload = await getResponse.Content.ReadFromJsonAsync<GetExamplePayload>();
        Assert.Equal(created.Item.Id, payload!.Item.Id);
        Assert.Equal("Lookup test", payload.Item.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateExample_WithInvalidName_ReturnsBadRequest(string name)
    {
        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/examples",
            new { name = "Valid name" });
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateExamplePayload>();
        Assert.NotNull(created);

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/v1/examples/{created!.Item.Id}",
            new { name });

        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
    }

    [Fact]
    public async Task GetExampleById_WhenNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/v1/examples/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateExample_WhenNotFound_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync(
            $"/api/v1/examples/{Guid.NewGuid()}",
            new { name = "Ghost" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExample_WhenNotFound_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"/api/v1/examples/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAndDeleteExample_Works()
    {
        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/examples",
            new { name = "Before update" });
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateExamplePayload>();
        Assert.NotNull(created);

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/v1/examples/{created!.Item.Id}",
            new { name = "After update" });

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/v1/examples/{created.Item.Id}");
        getResponse.EnsureSuccessStatusCode();
        var getPayload = await getResponse.Content.ReadFromJsonAsync<GetExamplePayload>();
        Assert.Equal("After update", getPayload!.Item.Name);

        var deleteResponse = await _client.DeleteAsync($"/api/v1/examples/{created.Item.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var notFoundResponse = await _client.GetAsync($"/api/v1/examples/{created.Item.Id}");
        Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
    }

    private sealed record ListExamplesPayload(IReadOnlyList<ExampleItemPayload> Items);

    private sealed record CreateExamplePayload(ExampleItemPayload Item);

    private sealed record GetExamplePayload(ExampleItemPayload Item);

    private sealed record ExampleItemPayload(Guid Id, string Name, DateTimeOffset CreatedAt);
}
