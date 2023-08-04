﻿using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Services
{
    public class RentalService : IRentalService
    {
        private const string entityType = "Rental";
        public readonly RentalDbContext dbContext;
        public readonly ILogger<RentalService> logger;
        public readonly ILogHandler logHandler;
        public readonly IMapper mapper;
        public RentalService(RentalDbContext dbContext, ILogger<RentalService> logger,ILogHandler logHandler , IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.logHandler = logHandler;
            this.mapper = mapper;
        }

        private static void NullRentalCheck(RentalOffice rentalOffice, int id)
        {
            if (rentalOffice == null)
            {
                throw new FileNotFoundException($"Rental with id {id} not found");
            }
        }

        public string CreateRental(CreateRentalOfficeDto dto)
        {
            this.logHandler.LogNewRequest(entityType, "post");

            var rentalOffice = this.mapper.Map<RentalOffice>(dto);
            this.dbContext.rentalOffices.Add(rentalOffice);
            this.dbContext.SaveChanges();

            this.logger.LogInformation("New Rental with id {0} created", rentalOffice.Id);
            string path = $"/api/rentaloffices/{rentalOffice.Id}";
            return path;
        }

        public void DeleteRental(int id)
        {
            this.logHandler.LogNewRequest(entityType, "delete");
            var rentalOffice = this.dbContext.rentalOffices
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            this.dbContext.rentalOffices.Remove(rentalOffice!);
            this.dbContext.SaveChanges();

            this.logger.LogWarning("New Rental with id {0} deleted", rentalOffice.Id);
            
        }

        public IEnumerable<RentalOfficeDto> GetRentalAll()
        {
            this.logHandler.LogNewRequest(entityType, "get");
            var rentals = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .ToList();

            var rentalsDto = this.mapper.Map<List<RentalOfficeDto>>(rentals);

            return rentalsDto;
        }

        public RentalOfficeDto GetRentalById(int id)
        {
            this.logHandler.LogNewRequest(entityType, "get");
            var rentalOffice = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            var rentalDto = this.mapper.Map<RentalOfficeDto>(rentalOffice);
            return rentalDto;

        }

        public void PutRentalById(RentalOfficeUpdateDto dto, int id)
        {
            this.logHandler.LogNewRequest(entityType, "put");

            var rentalOffice = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            rentalOffice.Name = dto.Name;
            rentalOffice.Description = dto.Description;
            rentalOffice.Category = dto.Category;
            rentalOffice.AcceptUnder23 = dto.AcceptUnder23;
            rentalOffice.ConntactEmail = dto.ConntactEmail;
            rentalOffice.ConntactNumber = dto.ConntactNumber;

            this.dbContext.SaveChanges();

            this.logger.LogInformation("New Rental with id {0} created", rentalOffice.Id);
        }

        
    }
}