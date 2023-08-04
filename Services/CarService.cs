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
    }
}
