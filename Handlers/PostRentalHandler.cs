using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using System.Reflection.Metadata.Ecma335;

namespace CarRentalAPI.Handlers
{
    public class PostRentalHandler : IPostRentalHandler
    {
        public readonly RentalDbContext dbContext;
        public readonly IMapper mapper;
        public PostRentalHandler(RentalDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public string HandlePostRental(CreateRentalOfficeDto dto) 
        {
            var rentalOffice = this.mapper.Map<RentalOffice>(dto);
            this.dbContext.rentalOffices.Add(rentalOffice);
            this.dbContext.SaveChanges();

            string path = $"/api/rentaloffices/{rentalOffice.Id}";
            return path;
        }

        
    }
}
