using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RestaurantApi.Tests;

public class RestaurantsApiTests : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client;

    public RestaurantsApiTests(TestApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_returns_201_with_audit_fields()
    {
        var response = await _client.PostAsJsonAsync("/api/restaurants", TestData.Restaurant("Created Diner", "1 Made St"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.NotNull(body);
        Assert.True(body!.ContainsKey("createdAt"));
        Assert.True(body!.ContainsKey("updatedAt"));
    }

    [Fact]
    public async Task Create_duplicate_case_different_returns_409()
    {
        await _client.PostAsJsonAsync("/api/restaurants", TestData.Restaurant("Dup Diner", "2 Made St"));

        var response = await _client.PostAsJsonAsync("/api/restaurants", TestData.Restaurant("DUP DINER", "2 Made St"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Create_missing_fields_returns_400()
    {
        var response = await _client.PostAsJsonAsync("/api/restaurants", new { name = "", address = "", contactNumber = "", hoursOfOperation = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(Skip = "EF.Functions.ILike is Postgres-specific; not supported by EF InMemory provider")]
    public async Task Search_partial_case_insensitive()
    {
        await _client.PostAsJsonAsync("/api/restaurants", TestData.Restaurant("Searchable Pasta", "5 Find St"));

        var response = await _client.GetAsync("/api/restaurants?name=SEARCH");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();
        Assert.NotNull(body);
        Assert.Contains(body!, r => r["name"].ToString()!.Contains("Pasta"));
    }

    [Fact(Skip = "EF.Functions.ILike is Postgres-specific; not supported by EF InMemory provider")]
    public async Task Search_no_match_returns_200_with_empty_array()
    {
        var response = await _client.GetAsync("/api/restaurants?name=zzznomatch");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(body);
        Assert.Empty(body!);
    }
}
