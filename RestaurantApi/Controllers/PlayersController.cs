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
    [ProducesResponseType(typeof(PlayerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PlayerResponse>> Create(
        [FromBody] CreatePlayerRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}
