using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Dtos;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _service;

    public PlayersController(IPlayerService service)
    {
        _service = service;
    }

    [HttpPost]
    [EndpointSummary("Save a player. Returns 409 on duplicate email, passport, or driver's license.")]
    [ProducesResponseType(typeof(PlayerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PlayerResponse>> Create(
        [FromBody] CreatePlayerRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return Created($"/api/players/{response.Id}", response);
    }

    [HttpGet]
    [EndpointSummary("Partial, case-insensitive search by firstName / lastName (either or both).")]
    [ProducesResponseType(typeof(IReadOnlyList<PlayerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<PlayerResponse>>> SearchByName(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        CancellationToken cancellationToken)
    {
        var results = await _service.SearchByNameAsync(firstName, lastName, cancellationToken);
        return Ok(results);
    }
}
