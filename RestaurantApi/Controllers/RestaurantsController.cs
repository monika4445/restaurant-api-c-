using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Dtos;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _service;

    public RestaurantsController(IRestaurantService service)
    {
        _service = service;
    }

    [HttpPost]
    [EndpointSummary("Save a restaurant. Returns 409 on duplicate Name+Address (case-insensitive).")]
    [ProducesResponseType(typeof(RestaurantResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RestaurantResponse>> Create(
        [FromBody] CreateRestaurantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return Created($"/api/restaurants/{response.Id}", response);
    }

    [HttpGet]
    [EndpointSummary("Partial, case-insensitive search by restaurant name.")]
    [ProducesResponseType(typeof(IReadOnlyList<RestaurantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<RestaurantResponse>>> SearchByName(
        [FromQuery, Required, DefaultValue("Sample")] string name,
        CancellationToken cancellationToken)
    {
        var results = await _service.SearchByNameAsync(name, cancellationToken);
        return Ok(results);
    }

    [HttpGet("members")]
    [EndpointSummary("Restaurants matching name + count of members aged >= age.")]
    [ProducesResponseType(typeof(IReadOnlyList<RestaurantWithMemberCountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<RestaurantWithMemberCountResponse>>> GetMembersAged(
        [FromQuery, Required, DefaultValue("Sample")] string name,
        [FromQuery, Required, Range(0, 150), DefaultValue(18)] int age,
        CancellationToken cancellationToken)
    {
        var results = await _service.GetMembersAgedAsync(name, age, cancellationToken);
        return Ok(results);
    }
}
