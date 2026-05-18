using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RestaurantApi.Tests;

public class PlayersApiTests : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client;

    public PlayersApiTests(TestApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_returns_201()
    {
        var response = await _client.PostAsJsonAsync("/api/players", TestData.Player(email: "p1@example.com", license: "DL-P1", passport: "PA-P1"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_with_future_dob_returns_400()
    {
        var response = await _client.PostAsJsonAsync("/api/players", TestData.Player(email: "fut@example.com", license: "DL-F", passport: "PA-F", dob: "2099-01-01"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_with_duplicate_email_case_different_returns_409()
    {
        await _client.PostAsJsonAsync("/api/players", TestData.Player(email: "dup@example.com", license: "DL-D1", passport: "PA-D1"));

        var response = await _client.PostAsJsonAsync("/api/players", TestData.Player(firstName: "Other", lastName: "User", email: "DUP@example.com", license: "DL-D2", passport: "PA-D2"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
