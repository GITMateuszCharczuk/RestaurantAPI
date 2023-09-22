using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IDishService
{
    public int Create(int restaurantId, CreateDishDto dto);
    public IEnumerable<DishDto> GetAll(int restaurantId);
    public DishDto Get(int restaurantId, int dishId);
    void Delete(int restaurantId, int dishId);
    void DeleteAll(int restaurantId);
}