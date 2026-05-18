using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Dtos;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers;

[ApiController]
[Route("api/memberships")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipService _service;

    public MembershipsController(IMembershipService service)
    {
        _service = service;
    }

    [HttpPost]
    [EndpointSummary("Make a player a member of a restaurant. 404 if either is missing, 409 on duplicate.")]
    [ProducesResponseType(typeof(MembershipResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MembershipResponse>> Create(
        [FromBody] CreateMembershipRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return Created($"/api/memberships/{response.Id}", response);
    }

    [HttpGet]
    [EndpointSummary("Player's memberships (exact name match). Each restaurant has isFavoriteRestaurant flag.")]
    [ProducesResponseType(typeof(IReadOnlyList<PlayerMembershipsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<PlayerMembershipsResponse>>> GetByPlayerName(
        [FromQuery, Required] string firstName,
        [FromQuery, Required] string lastName,
        CancellationToken cancellationToken)
    {
        var results = await _service.GetByPlayerNameAsync(firstName, lastName, cancellationToken);
        return Ok(results);
    }
}
