using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Dtos;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers;

[ApiController]
[Route("api/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _service;

    public FavoritesController(IFavoriteService service)
    {
        _service = service;
    }

    [HttpPost]
    [EndpointSummary("Mark a restaurant as a player's favorite. 404 if either is missing, 409 on duplicate.")]
    [ProducesResponseType(typeof(FavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FavoriteResponse>> Create(
        [FromBody] CreateFavoriteRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return Created($"/api/favorites/{response.Id}", response);
    }

    [HttpGet]
    [EndpointSummary("Player's favorites (exact name match). Each restaurant has 'linked' flag (= also a member).")]
    [ProducesResponseType(typeof(IReadOnlyList<PlayerFavoritesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<PlayerFavoritesResponse>>> GetByPlayerName(
        [FromQuery, Required, DefaultValue("Ada")] string firstName,
        [FromQuery, Required, DefaultValue("Lovelace")] string lastName,
        CancellationToken cancellationToken)
    {
        var results = await _service.GetByPlayerNameAsync(firstName, lastName, cancellationToken);
        return Ok(results);
    }
}
