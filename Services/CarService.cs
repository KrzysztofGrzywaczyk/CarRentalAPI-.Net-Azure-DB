using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CarRentalAPI.Entities.Car;

namespace CarRentalAPI.Services;

public class CarService : ICarService
{
    private const string entityType = "Car";
    private readonly RentalDbContext _dbContext;
    private readonly ILogHandler _logHandler;
    private readonly IMapper _mapper;

    public CarService(RentalDbContext dbContext, ILogHandler logHandler, IMapper mapper)
    {
        _dbContext = dbContext;
        _logHandler = logHandler;
        _mapper = mapper;
    }

    public string CreateCar(int rentalId, CreateCarDto dto)
    {
        _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.POST);

        var rentalOffice = LoadRentalOfficeIfExist(rentalId); 

        var carEntity = _mapper.Map<Car>(dto);
        carEntity.RentalOfficeId = rentalOffice.Id;

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

        _dbContext.cars.Remove(carEntity);
        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Deleted, carEntity.Id);

    }

    public IEnumerable<PresentCarAllCarsDto> GetAllCarsInBase()
    {
        _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);

        var cars = _dbContext.cars
            .ToList();

        var carDtos = _mapper.Map<List<PresentCarAllCarsDto>>(cars);

        return carDtos;
    }

    public IEnumerable<PresentCarDto> GetAllCarsInRental(int rentalId)
    {
        _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);

        var rentalOffice = LoadRentalOfficeIfExist(rentalId);

        var cars = _dbContext.cars
            .Where(c => c.RentalOfficeId == rentalId)
            .ToList();

        var carDtos = _mapper.Map<List<PresentCarDto>>(cars);

        return carDtos;
    }

    public PresentCarDto GetCarById(int rentalId, int carId) 
    {
        _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);

        LoadRentalOfficeIfExist(rentalId);

        var carEntity = GetCarIfExist(rentalId, carId);

        var carDto = _mapper.Map<PresentCarDto>(carEntity);

        return carDto;
    }

    public string PutCar(int rentalId, int carId, CreateCarDto dto)
    {
        _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.PUT);

        LoadRentalOfficeIfExist(rentalId);

        var carEntity = _dbContext.cars.FirstOrDefault(c => c.RentalOfficeId == rentalId && c.Id == carId);

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
