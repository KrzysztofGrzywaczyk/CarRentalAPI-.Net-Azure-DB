using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Handlers
{
    public class PutRentalHandler : IPutRentalHandler
    {
        public readonly RentalDbContext dbContext;
        public readonly IMapper mapper;
        public PutRentalHandler(RentalDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public bool HandlePutById(RentalOfficeUpdateDto dto, int id) 
        {
            var rental = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            if (rental == null)
            {
                return false;
            }

            rental.Name = dto.Name;
            rental.Description = dto.Description;
            rental.Category = dto.Category;
            rental.AcceptUnder23 = dto.AcceptUnder23;
            rental.ConntactEmail = dto.ConntactEmail;
            rental.ConntactNumber = dto.ConntactNumber;

            this.dbContext.SaveChanges();

            return true;

        }
    }
}
