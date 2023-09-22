using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI;

public class RestaurantSeeder
{
    private readonly RestaurantDbContext _restaurantDbContext;

    public RestaurantSeeder(RestaurantDbContext restaurantDbContext)
    {
        _restaurantDbContext = restaurantDbContext;
    }

    public void Seed()
    {
        
        if (_restaurantDbContext.Database.CanConnect())
        {
            if (!_restaurantDbContext.Roles.Any())
            {
                var roles = GetRoles();
                _restaurantDbContext.Roles.AddRange(roles);
                _restaurantDbContext.SaveChanges();
            }
        }
        
        if (_restaurantDbContext.Database.CanConnect())
        {
            if (!_restaurantDbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                _restaurantDbContext.Restaurants.AddRange(restaurants);
                _restaurantDbContext.SaveChanges();
            }
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        var restaurants = new List<Restaurant>()
        {
            new Restaurant()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                ContactNumber = "555555555",
                HasDelivery = true,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "asdasd",
                        Price = 10.30,
                    },

                    new Dish()
                    {
                        Name = "Chicken Nuggets",
                        Description = "asdasd",
                        Price = 5.30,
                    },
                },
                Address = new Address()
                {
                    City = "Kraków",
                    Street = "Długa 5",
                    PostalCode = "30-001"
                }
            },
            new Restaurant()
            {
                Name = "McDonald Szewska",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactEmail = "contact@mcdonald.com",
                ContactNumber = "555555555",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "Kraków",
                    Street = "Szewska 2",
                    PostalCode = "30-001"
                }
            }
        };

        return restaurants;
    }

    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Manager"
            },
            new Role()
            {
                Name = "Admin"
            }
        };

        return roles;
    }
}