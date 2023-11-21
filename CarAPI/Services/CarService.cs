using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Services
{
    public class CarService : ICarService
    {
        private const string entityType = "Car";
        private readonly RentalDbContext dbContext;
        private readonly ILogHandler logHandler;
        private readonly IMapper mapper;

        public CarService(RentalDbContext dbContext, ILogHandler logHandler, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logHandler = logHandler;
            this.mapper = mapper;
        }

        public string CreateCar(int rentalID, CreateCarDto dto)
        {
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.POST);

            var rentalOffice = this.dbContext.rentalOffices.FirstOrDefault(r => r.Id == rentalID);
            if (rentalOffice is null)
            {
                throw new NotFoundException("Not found rental office with given this id.");
            }    

            var carEntity = this.mapper.Map<Car>(dto);
            carEntity.RentalOfficeId = rentalOffice.Id;

            this.dbContext.cars.Add(carEntity);
            this.dbContext.SaveChanges();

            this.logHandler.LogAction(ILogHandler.ActionEnum.Created, carEntity.Id);

            var path = $"/api/{rentalID}/cars/{carEntity.Id}";
            return path;
        }

        public IEnumerable<CarDto> GetCarAll(int rentalId)
        {
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);

            var cars = this.dbContext.cars
                .Where(c => c.RentalOfficeId == rentalId)
                .ToList();

            var carDtos = this.mapper.Map<List<CarDto>>(cars);

            return carDtos;
        }

        public CarDto GetCarById(int rentalId, int carId) 
        {
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);

            var car = this.dbContext.cars.
                FirstOrDefault(c => (c.RentalOfficeId == rentalId && c.Id == carId));

            var carDto = this.mapper.Map<CarDto>(car);

            return carDto;

        }
    }
}
