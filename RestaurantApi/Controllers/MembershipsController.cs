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
    [ProducesResponseType(typeof(MembershipResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MembershipResponse>> Create(
        [FromBody] CreateMembershipRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet]
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
