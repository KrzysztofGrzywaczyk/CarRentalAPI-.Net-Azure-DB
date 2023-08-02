using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Handlers
{
    public class GetRentalHandler : IGetRentalHandler
    {
        public readonly RentalDbContext dbContext;
        public readonly IMapper mapper;
        public GetRentalHandler(RentalDbContext dbContext, IMapper mapper) 
        { 
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public IEnumerable<RentalOfficeDto> HandleGetAllRequest() 
        {
            var rentals = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .ToList();

            var rentalsDto = this.mapper.Map<List<RentalOfficeDto>>(rentals);

            return rentalsDto;
        }
        public RentalOfficeDto HandleGetByIdRequest(int id)
        {
            var rental = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            if (rental != null)
            {
                var rentalDto = this.mapper.Map<RentalOfficeDto>(rental);
                return rentalDto;
            }
            return null!;
        }
    }
}
