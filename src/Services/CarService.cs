using AutoMapper;
using CarRentalAPI.Authorization;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Models.Queries;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using static CarRentalAPI.Entities.Car;

namespace CarRentalAPI.Services;

public class CarService(RentalDbContext dbContext, ILogHandler logHandler, IMapper mapper,
        IAuthorizationService authorizationService, IUserContextService userContextService) : ICarService
{
    private const string entityName = "Car";

    public string CreateCar(int rentalId, CreateCarDto dto)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.POST);

        var rentalOffice = LoadRentalOfficeIfExist(rentalId);
        var createdByUser = userContextService.GetUserId;
        var managedByUserId = rentalOffice.OwnerId;

        var carEntity = mapper.Map<Car>(dto);
        carEntity.RentalOfficeId = rentalOffice.Id;
        carEntity.CreatedById = createdByUser;
        carEntity.ManagedById = managedByUserId;


        dbContext.cars.Add(carEntity);
        dbContext.SaveChanges();

        logHandler.LogAction(ILogHandler.ActionEnum.Created, carEntity.Id);

        var path = $"/api/{rentalId}/cars/{carEntity.Id}";
        return path;
    }

    public void DeleteCar(int rentalId, int carId)
    {
        LoadRentalOfficeIfExist(rentalId);
        
        var carEntity = GetCarIfExist(rentalId, carId);

        var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, carEntity, new ResourceOperationRequirement(ResourceOperation.Delete));

        if (!authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }

        dbContext.cars.Remove(carEntity);
        dbContext.SaveChanges();

        logHandler.LogAction(ILogHandler.ActionEnum.Deleted, carEntity.Id);

    }

    public PagedResult<PresentCarAllCarsDto> GetAllCarsInBase(CarQuery query)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        var fullQuery = dbContext
            .cars
            .Where(c => query.SearchPhrase == null ||
            (c.Brand!.ToLower().Contains(query.SearchPhrase.ToLower()) || c.Model!.ToLower().Contains(query.SearchPhrase.ToLower())));

        var count = fullQuery.Count();

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var selector = new Dictionary<string, Expression<Func<Car, object>>>
            {
                { nameof(Car.Brand), c => c.Brand! },
                { nameof(Car.Model), c => c.Model! },
                { nameof(Car.Year), c => c.Year },
                { nameof(Car.Fuel), c => c.Fuel },
                { nameof(Car.Segment), c => c.Segment },
                { nameof(Car.RentalOfficeId), c => c.RentalOfficeId }
            };

            var selectedColumn = selector[query.SortBy];

            fullQuery = query.SortDirection == SortDirection.Ascending ? fullQuery.OrderBy(selectedColumn) : fullQuery.OrderByDescending(selectedColumn);
        }

        var pagedQuery = fullQuery
            .Skip(query.PageSize*(query.PageNumber -1))
            .Take(query.PageSize)
            .ToList();

        var carDtos = mapper.Map<List<PresentCarAllCarsDto>>(pagedQuery);

        var pagedResult = new PagedResult<PresentCarAllCarsDto>(carDtos, count, query.PageSize, query.PageNumber);

        return pagedResult;
    }

    public PagedResult<PresentCarDto> GetAllCarsInRental(int rentalId, CarQuery query)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        var rentalOffice = LoadRentalOfficeIfExist(rentalId);

        var fullQuery = dbContext.cars
            .Where(c => c.RentalOfficeId == rentalId)
            .Where(c => query.SearchPhrase == null ||
            (c.Brand!.ToLower().Contains(query.SearchPhrase.ToLower()) || c.Model!.ToLower().Contains(query.SearchPhrase.ToLower())));

        var count = fullQuery.Count();

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var selector = new Dictionary<string, Expression<Func<Car, object>>>
            {
                { nameof(Car.Brand), c => c.Brand! },
                { nameof(Car.Model), c => c.Model! },
                { nameof(Car.Year), c => c.Year },
                { nameof(Car.Fuel), c => c.Fuel },
                { nameof(Car.Segment), c => c.Segment },
                { nameof(Car.RentalOfficeId), c => c.RentalOfficeId }
            };

            var selectedColumn = selector[query.SortBy];

            fullQuery = query.SortDirection == SortDirection.Ascending ? fullQuery.OrderBy(selectedColumn) : fullQuery.OrderByDescending(selectedColumn);
        }

        var pagedQuery = fullQuery
           .Skip(query.PageSize * (query.PageNumber - 1))
           .Take(query.PageSize)
           .ToList();

        var carDtos = mapper.Map<List<PresentCarDto>>(pagedQuery);

        var pagedResult = new PagedResult<PresentCarDto>(carDtos, count, query.PageSize, query.PageNumber);

        return pagedResult;
    }

    public PresentCarDto GetCarById(int rentalId, int carId) 
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        LoadRentalOfficeIfExist(rentalId);

        var carEntity = GetCarIfExist(rentalId, carId);

        var carDto = mapper.Map<PresentCarDto>(carEntity);

        return carDto;
    }

    public string PutCar(int rentalId, int carId, CreateCarDto dto)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.PUT);

        LoadRentalOfficeIfExist(rentalId);

        var carEntity = dbContext.cars.FirstOrDefault(c => c.RentalOfficeId == rentalId && c.Id == carId);

        var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, carEntity, new ResourceOperationRequirement(ResourceOperation.Update));

        if (authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }


        carEntity!.PlateNumber = dto.PlateNumber;
        carEntity!.Brand = dto.Brand;
        carEntity!.Model = dto.Model;
        carEntity!.Year = dto.Year;
        carEntity!.Fuel = (FuelType)Enum.Parse(typeof(FuelType), dto.Fuel!, true);
        carEntity!.Segment = dto.Segment;

        dbContext.SaveChanges();

        logHandler.LogAction(ILogHandler.ActionEnum.Updated, rentalId, carId);

        var path = $"/api/{rentalId}/cars/{carEntity.Id}";

        return path;
    }

    private RentalOffice LoadRentalOfficeIfExist(int rentalId)
    {
        var renatlOffice = dbContext.rentalOffices.FirstOrDefault(r => r.Id == rentalId);
        if (renatlOffice is null)
        {
            throw new NotFoundException("Not found rental office with given id.");
        }
        return renatlOffice;
    }

    private Car GetCarIfExist(int rentalId, int carId)
    {
        var carEntity = dbContext.cars.FirstOrDefault(c => c.RentalOfficeId == rentalId && c.Id == carId);
        if (carEntity is null)
        {
            throw new NotFoundException("Not found car with given Id in given rental.");
        }
        return carEntity;
    }
}