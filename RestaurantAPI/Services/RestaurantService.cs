﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;

    public RestaurantService(RestaurantDbContext dbContext, IMapper mapper,
        ILogger<RestaurantService> logger,
        IAuthorizationService authorizationService
        ,IUserContextService userContextService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
        _mapper = mapper;
        _dbContext = dbContext;
    }
    
    public RestaurantDto GetById(int id)
    {
        var restaurant = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(x => x.Id == id);

        if (restaurant is null)
        {
            throw new NotFoundException("Restaurant not Found");
        }

        var result = _mapper.Map<RestaurantDto>(restaurant);
        return result;
    }

    public PageResult<RestaurantDto> GetAll(RestaurantQuery query)
    {
        var baseQuery = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => query.SearchPhrase == null || r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                       r.Description.ToLower().Contains(query.SearchPhrase.ToLower()));

        
        var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var totalItemsCount = baseQuery.Count();
        
        var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
        
        var pageResult = new PageResult<RestaurantDto>(restaurantsDtos, totalItemsCount , query.PageSize, query.PageNumber);

        return pageResult;
    }

    public int Create(CreateRestaurantDto dto)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        restaurant.CreatedById = _userContextService.GetUserId;
        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();
        
        return restaurant.Id;
    }

    public void Delete(int id)
    {
        _logger.LogWarning("Restaurant with id: {Id} DELETE action invoked", id);
        
        
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(x => x.Id == id);

        if (restaurant is null)
        {
            throw new NotFoundException("Restaurant not Found");
        }
        
        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException("");
        }
        
        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();
        
    }

    public void Modify(int id, ModifyRestaurantDto dto)
    {
        
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(x => x.Id == id);
        
        if (restaurant is null)
        {
            throw new NotFoundException("Restaurant not Found");
        }

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException("");
        }
        
        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;
        
        _dbContext.SaveChanges();

    }

    
}