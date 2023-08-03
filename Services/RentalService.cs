using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace CarRentalAPI.Services
{
    public class RentalService : IRentalService
    {
        public readonly RentalDbContext dbContext;
        public readonly IMapper mapper;
        public readonly ILogger<RentalService> logger;
        public RentalService(RentalDbContext dbContext, ILogger<RentalService> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        public string CreateRental(CreateRentalOfficeDto dto)
        {
            this.logger.LogInformation("Rental POST request received.");

            var rentalOffice = this.mapper.Map<RentalOffice>(dto);
            this.dbContext.rentalOffices.Add(rentalOffice);
            this.dbContext.SaveChanges();

            this.logger.LogInformation("New Rental with id {0} created", rentalOffice.Id);
            string path = $"/api/rentaloffices/{rentalOffice.Id}";
            return path;
        }

        public bool DeleteRental(int id)
        {
            this.logger.LogInformation("Rental DELETE request received.");
            var rentalOffice = this.dbContext.rentalOffices
                .FirstOrDefault(r => r.Id == id);
            if (rentalOffice == null)
            {
                return false;

            }

            this.dbContext.rentalOffices.Remove(rentalOffice);
            this.dbContext.SaveChanges();

            this.logger.LogWarning("New Rental with id {0} deleted", rentalOffice.Id);

            return true;
        }

        public IEnumerable<RentalOfficeDto> GetRentalAll()
        {
            this.logger.LogInformation("Rental GET request received.");
            var rentals = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .ToList();

            var rentalsDto = this.mapper.Map<List<RentalOfficeDto>>(rentals);

            return rentalsDto;
        }

        public RentalOfficeDto GetRentalById(int id)
        {
            this.logger.LogInformation("Rental GET request received.");
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

        public bool PutRentalById(RentalOfficeUpdateDto dto, int id)
        {
            this.logger.LogInformation("Rental PUT request received.");

            var rentalOffice = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            if (rentalOffice == null)
            {
                return false;
            }

            rentalOffice.Name = dto.Name;
            rentalOffice.Description = dto.Description;
            rentalOffice.Category = dto.Category;
            rentalOffice.AcceptUnder23 = dto.AcceptUnder23;
            rentalOffice.ConntactEmail = dto.ConntactEmail;
            rentalOffice.ConntactNumber = dto.ConntactNumber;

            this.dbContext.SaveChanges();

            this.logger.LogInformation("New Rental with id {0} created", rentalOffice.Id);

            return true;
        }
    }
}