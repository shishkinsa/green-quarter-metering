using System.Net;
using System.Net.Http.Json;

namespace GQ.WebApi.Tests.Integration;

[Collection("api")]
public sealed class ApartmentsEndpointTests(WebApiFactory factory): IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpsertOwner_Works()
    {
        var apartmentId = Guid.Parse("c0000001-0000-0000-0000-000000000002");
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/v1/apartments/{apartmentId}/owner",
            new { fullName = "Петров Пётр Петрович", phone = "+79007654321" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        UpsertOwnerPayload? payload = await response.Content.ReadFromJsonAsync<UpsertOwnerPayload>();
        Assert.NotNull(payload);
        Assert.Equal("Петров Пётр Петрович", payload!.Item.FullName);
    }

    [Fact]
    public async Task UpsertOwner_WhenApartmentNotFound_Returns404()
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/v1/apartments/{Guid.NewGuid()}/owner",
            new { fullName = "Test", phone = (string?)null });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpsertOwner_WithInvalidName_Returns400(string fullName)
    {
        var apartmentId = Guid.Parse("c0000001-0000-0000-0000-000000000001");
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/v1/apartments/{apartmentId}/owner",
            new { fullName, phone = (string?)null });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private sealed record UpsertOwnerPayload(OwnerPayload Item);

    private sealed record OwnerPayload(Guid Id, Guid ApartmentId, string FullName, string? Phone);
}
