using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Services;

namespace RestaurantAPI.Authorization;

public class MinimumCreatedRestaurantsHandler : AuthorizationHandler<MinimumCreatedRestaurants>
{
    private readonly RestaurantDbContext _dbContext;


    public MinimumCreatedRestaurantsHandler(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumCreatedRestaurants requirement)
    {
        var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

        var createdEnough = CreatedRestaurantsCount(requirement.MinimumCreatedRestaurantsCount, userId);

        if (createdEnough)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
    
    private bool CreatedRestaurantsCount(int restaurantsCount, int userId)
    {
        var restaurants = _dbContext
            .Restaurants
            .Include(r => r.CreatedBy)
            .ToList();
        // throw new NotFoundException($"{restaurants[1].CreatedBy.Id}");
        if (restaurants is null || !restaurants.Any())
        {
            throw new NotFoundException("No restaurants found!!!");
        }
        
        var createdRestaurantsCount = restaurants!.
            FindAll(r => r.CreatedById == userId).Count;
        
        if ( createdRestaurantsCount >= restaurantsCount)
        {
            return true;
        }
        
        return false;
    }
}