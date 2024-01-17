using AutoMapper;
using CarRentalAPI.Authorization;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CarRentalAPI.Entities.Car;

namespace CarRentalAPI.Services;

public class CarService : ICarService
{
    private const string entityName = "Car";
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;
    private readonly RentalDbContext _dbContext;
    private readonly ILogHandler _logHandler;
    private readonly IMapper _mapper;

    public CarService(RentalDbContext dbContext, ILogHandler logHandler, IMapper mapper,
        IAuthorizationService authorizationService, IUserContextService userContextService)
    {
        _authorizationService = authorizationService;
        _dbContext = dbContext;
        _logHandler = logHandler;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public string CreateCar(int rentalId, CreateCarDto dto)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.POST);

        var rentalOffice = LoadRentalOfficeIfExist(rentalId);
        var createdByUser = _userContextService.GetUserId;
        var managedByUserId = rentalOffice.OwnerId;

        var carEntity = _mapper.Map<Car>(dto);
        carEntity.RentalOfficeId = rentalOffice.Id;
        carEntity.CreatedById = createdByUser;
        carEntity.ManagedById = managedByUserId;


        _dbContext.cars.Add(carEntity);
        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Created, carEntity.Id);

        var path = $"/api/{rentalId}/cars/{carEntity.Id}";
        return path;
    }

    public void DeleteCar(int rentalId, int carId)
    {
        LoadRentalOfficeIfExist(rentalId);
        
        var carEntity = GetCarIfExist(rentalId, carId);

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, carEntity, new ResourceOperationRequirement(ResourceOperation.Delete));

        if (!authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }

        _dbContext.cars.Remove(carEntity);
        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Deleted, carEntity.Id);

    }

    public IEnumerable<PresentCarAllCarsDto> GetAllCarsInBase()
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        var cars = _dbContext.cars
            .ToList();

        var carDtos = _mapper.Map<List<PresentCarAllCarsDto>>(cars);

        return carDtos;
    }

    public IEnumerable<PresentCarDto> GetAllCarsInRental(int rentalId)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        var rentalOffice = LoadRentalOfficeIfExist(rentalId);

        var cars = _dbContext.cars
            .Where(c => c.RentalOfficeId == rentalId)
            .ToList();

        var carDtos = _mapper.Map<List<PresentCarDto>>(cars);

        return carDtos;
    }

    public PresentCarDto GetCarById(int rentalId, int carId) 
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        LoadRentalOfficeIfExist(rentalId);

        var carEntity = GetCarIfExist(rentalId, carId);

        var carDto = _mapper.Map<PresentCarDto>(carEntity);

        return carDto;
    }

    public string PutCar(int rentalId, int carId, CreateCarDto dto)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.PUT);

        LoadRentalOfficeIfExist(rentalId);

        var carEntity = _dbContext.cars.FirstOrDefault(c => c.RentalOfficeId == rentalId && c.Id == carId);

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, carEntity, new ResourceOperationRequirement(ResourceOperation.Update));

        if (authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }


        carEntity!.PlateNumber = dto.PlateNumber;
        carEntity!.Brand = dto.Brand;
        carEntity!.Model = dto.Model;
        carEntity!.Year = dto.Year;
        carEntity!.Fuel = (FuelType)Enum.Parse(typeof(FuelType), dto.Fuel, true);
        carEntity!.Segment = dto.Segment;

        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Updated, rentalId, carId);

        var path = $"/api/{rentalId}/cars/{carEntity.Id}";

        return path;
    }

    private RentalOffice LoadRentalOfficeIfExist(int rentalId)
    {
        var renatlOffice = _dbContext.rentalOffices.FirstOrDefault(r => r.Id == rentalId);
        if (renatlOffice is null)
        {
            throw new NotFoundException("Not found rental office with given id.");
        }
        return renatlOffice;
    }

    private Car GetCarIfExist(int rentalId, int carId)
    {
        var carEntity = _dbContext.cars.FirstOrDefault(c => c.RentalOfficeId == rentalId && c.Id == carId);
        if (carEntity is null)
        {
            throw new NotFoundException("Not found car with given Id in given rental.");
        }
        return carEntity;
    }
}
