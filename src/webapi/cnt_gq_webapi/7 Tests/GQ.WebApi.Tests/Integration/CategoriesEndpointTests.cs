using System.Net;
using System.Net.Http.Json;

namespace GQ.WebApi.Tests.Integration;

[Collection("api")]
public sealed class CategoriesEndpointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ListCategories_ReturnsSeededItems()
    {
        var response = await _client.GetAsync("/api/v1/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ListCategoriesPayload>();
        Assert.NotNull(payload);
        Assert.True(payload!.Items.Count >= 3);
        Assert.Contains(payload.Items, x => x.Code == "demo");
    }

    private sealed record ListCategoriesPayload(IReadOnlyList<CategoryPayload> Items);

    private sealed record CategoryPayload(Guid Id, string Code, string Name);
}
