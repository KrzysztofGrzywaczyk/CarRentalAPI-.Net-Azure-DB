using AutoMapper;
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
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.POST);

            var rentalOffice = this.mapper.Map<RentalOffice>(dto);
            this.dbContext.rentalOffices.Add(rentalOffice);
            this.dbContext.SaveChanges();

            this.logHandler.LogAction(ILogHandler.ActionEnum.Created, rentalOffice.Id);

            string path = $"/api/rentaloffices/{rentalOffice.Id}";
            return path;
        }

        public void DeleteRental(int id)
        {
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.DELETE);
            var rentalOffice = this.dbContext.rentalOffices
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            this.dbContext.rentalOffices.Remove(rentalOffice!);
            this.dbContext.SaveChanges();

            this.logHandler.LogAction(ILogHandler.ActionEnum.Deleted, rentalOffice.Id); ;
            
        }

        public IEnumerable<RentalOfficeDto> GetRentalAll()
        {
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);
            var rentals = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .ToList();

            var rentalDtos = this.mapper.Map<List<RentalOfficeDto>>(rentals);

            return rentalDtos;
        }

        public RentalOfficeDto GetRentalById(int id)
        {
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);
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
            this.logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.PUT);

            var rentalOffice = this.dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            rentalOffice!.Name = dto.Name;
            rentalOffice.Description = dto.Description;
            rentalOffice.Category = dto.Category;
            rentalOffice.AcceptUnder23 = dto.AcceptUnder23;
            rentalOffice.ConntactEmail = dto.ConntactEmail;
            rentalOffice.ConntactNumber = dto.ConntactNumber;

            this.dbContext.SaveChanges();

            this.logHandler.LogAction(ILogHandler.ActionEnum.Updated, rentalOffice.Id);
        }

        
    }
}