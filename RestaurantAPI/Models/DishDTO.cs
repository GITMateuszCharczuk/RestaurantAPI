namespace RestaurantAPI.Models;

public class DishDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Double? Price { get; set; }
}