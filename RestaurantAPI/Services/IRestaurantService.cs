using System.Security.Claims;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    RestaurantDto GetById(int id);
    PageResult<RestaurantDto> GetAll(RestaurantQuery query);
    int Create(CreateRestaurantDto dto);
    
    public void Delete(int id);
    
    public void Modify(int id, ModifyRestaurantDto dto);
    
}