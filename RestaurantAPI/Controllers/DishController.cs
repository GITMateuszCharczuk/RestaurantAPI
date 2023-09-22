using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant/{restaurantId:int}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }
    
    [HttpPost]
    public ActionResult Post([FromRoute] int restaurantId, [FromBody]CreateDishDto dto)
    {
        var newDishId = _dishService.Create(restaurantId, dto);

        return Created($"api/restaurant/{restaurantId}/dish/{newDishId}",null);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
    {
        var dishDtoList = _dishService.GetAll(restaurantId);

        return Ok(dishDtoList);
    }
    
    [HttpGet("{dishId:int}")]
    public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dishDto = _dishService.Get(restaurantId, dishId);

        return Ok(dishDto);
    }
    
    [HttpDelete("{dishId:int}")]
    public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        _dishService.Delete(restaurantId, dishId);
        
        return NoContent();
    }
    
    [HttpDelete]
    public ActionResult DeleteAll([FromRoute] int restaurantId)
    {
        _dishService.DeleteAll(restaurantId);
        
        return NoContent();
    }
}