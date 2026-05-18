using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RestaurantApi.Tests;

public class MembershipsApiTests : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client;

    public MembershipsApiTests(TestApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_returns_201_with_existing_player_and_restaurant()
    {
        var rest = await CreateRestaurantAsync("Mem Diner", "1 M St");
        var player = await CreatePlayerAsync(email: "mem1@example.com", license: "DL-M1", passport: "PA-M1");

        var response = await _client.PostAsJsonAsync("/api/memberships", new { playerId = player, restaurantId = rest });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_with_missing_player_returns_404()
    {
        var rest = await CreateRestaurantAsync("Mem 404 Diner", "2 M St");

        var response = await _client.PostAsJsonAsync("/api/memberships", new { playerId = Guid.Empty, restaurantId = rest });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_with_missing_restaurant_returns_404()
    {
        var player = await CreatePlayerAsync(email: "mem2@example.com", license: "DL-M2", passport: "PA-M2");

        var response = await _client.PostAsJsonAsync("/api/memberships", new { playerId = player, restaurantId = Guid.Empty });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_duplicate_returns_409()
    {
        var rest = await CreateRestaurantAsync("Dup Mem Diner", "3 M St");
        var player = await CreatePlayerAsync(email: "mem3@example.com", license: "DL-M3", passport: "PA-M3");
        await _client.PostAsJsonAsync("/api/memberships", new { playerId = player, restaurantId = rest });

        var response = await _client.PostAsJsonAsync("/api/memberships", new { playerId = player, restaurantId = rest });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    private async Task<Guid> CreateRestaurantAsync(string name, string address)
    {
        var resp = await _client.PostAsJsonAsync("/api/restaurants", TestData.Restaurant(name, address));
        var body = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        return Guid.Parse(body!["id"].ToString()!);
    }

    private async Task<Guid> CreatePlayerAsync(string email, string license, string passport)
    {
        var resp = await _client.PostAsJsonAsync("/api/players", TestData.Player(email: email, license: license, passport: passport));
        var body = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        return Guid.Parse(body!["id"].ToString()!);
    }
}
