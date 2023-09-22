using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class MinimumCreatedRestaurants : IAuthorizationRequirement
{
    
    public int MinimumCreatedRestaurantsCount { get;  }

    public MinimumCreatedRestaurants(int minimumCreatedRestaurants)
    {
        MinimumCreatedRestaurantsCount = minimumCreatedRestaurants;
    }

    
}