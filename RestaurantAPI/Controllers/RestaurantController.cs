using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
[Authorize]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    
    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    // [Authorize(Policy = "AtLeastCreated2")]
    [AllowAnonymous]
    public ActionResult<ICollection<RestaurantDto>> GetAll([FromQuery] RestaurantQuery query)
    {
        var result = _restaurantService.GetAll(query);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<RestaurantDto> GetRestaurant([FromRoute] int id)
    {
        var result = _restaurantService.GetById(id);

        if (id <= 0)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager", Policy = "AtLeast20")]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
        Console.Write(userId);
        var createdId = _restaurantService.Create(dto);

        return Created($"/api/restaurant/{createdId}", null);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRestaurant([FromRoute] int id)
    {
        _restaurantService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult ModifyRestaurant([FromRoute] int id, [FromBody] ModifyRestaurantDto dto)
    {

        _restaurantService.Modify(id, dto);

        return Ok();
    }
}