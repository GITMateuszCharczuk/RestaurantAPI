using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public class DishService : IDishService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;

    public DishService(RestaurantDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
    
    public int Create(int restaurantId, CreateDishDto dto)
    {
        var restaurant = GetRestaurant(restaurantId);

        var dishEntity = _mapper.Map<Dish>(dto);

        dishEntity.RestaurantId = restaurantId;
        _dbContext.Dishes.Add(dishEntity);
        _dbContext.SaveChanges();

        return dishEntity.Id;
    }

    public IEnumerable<DishDto> GetAll(int restaurantId)
    {
        var dishes = GetRestaurant(restaurantId).Dishes;
        
        if (dishes is null || !dishes.Any())
        {
            throw new NotFoundException("Dishes not found!!!");
        }

        var dishDtos = _mapper.Map<IEnumerable<DishDto>>(dishes);

        return dishDtos;
    }

    public DishDto Get(int restaurantId, int dishId)
    {
        var dish = GetDish(restaurantId, dishId);

        var dishDto = _mapper.Map<DishDto>(dish);

        return dishDto;
    }

    public void Delete(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurant(restaurantId);
        
        var dish = GetDish(restaurantId, dishId);

        restaurant.Dishes.Remove(dish);
        
        _dbContext.SaveChanges();
    }
    
    public void DeleteAll(int restaurantId)
    {
        var restaurant = GetRestaurant(restaurantId);
        
        _dbContext.RemoveRange(restaurant.Dishes);
        
        _dbContext.SaveChanges();
    }
    
    
    private Dish GetDish(int restaurantId, int dishId)
    {
        var restaurant = _dbContext.Restaurants
            .FirstOrDefault(r => r.Id == restaurantId);
        
        if (restaurant is null)
        {
            throw new NotFoundException("Restaurant not found!!!");
        }

        var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);
        
        if (dish is null || dish.RestaurantId != restaurantId)
        {
            throw new NotFoundException("Dish not found!!!");
        }
        
        return dish;
    }

    private Restaurant GetRestaurant(int restaurantId)
    {
        var restaurant = _dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == restaurantId);


        if (restaurant is null)
        {
            throw new NotFoundException("Restaurant not found!!!");
        }

        return restaurant;
    }
}