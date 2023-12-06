using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;
using static CarRentalAPI.Entities.RentalOffice;

namespace CarRentalAPI.Services
{
    public class RentalService : IRentalService
    {
        private const string entityType = "Rental";
        public readonly RentalDbContext _dbContext;
        public readonly ILogger<RentalService> _logger;
        public readonly ILogHandler _logHandler;
        public readonly IMapper _mapper;
        public RentalService(RentalDbContext dbContext, ILogger<RentalService> logger,ILogHandler logHandler , IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _logHandler = logHandler;
            _mapper = mapper;
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
            _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.POST);

            var rentalOffice = _mapper.Map<RentalOffice>(dto);
            _dbContext.rentalOffices.Add(rentalOffice);
            _dbContext.SaveChanges();

            _logHandler.LogAction(ILogHandler.ActionEnum.Created, rentalOffice.Id);

            string path = $"/api/rentaloffices/{rentalOffice.Id}";
            return path;
        }

        public void DeleteRental(int id)
        {
            _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.DELETE);
            var rentalOffice = _dbContext.rentalOffices
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            _dbContext.rentalOffices.Remove(rentalOffice!);
            _dbContext.SaveChanges();

            _logHandler.LogAction(ILogHandler.ActionEnum.Deleted, rentalOffice!.Id);
        }

        public IEnumerable<RentalOfficeDto> GetRentalAll()
        {
            _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);
            var rentals = _dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .ToList();

            var rentalDtos = _mapper.Map<List<RentalOfficeDto>>(rentals);

            return rentalDtos;
        }

        public RentalOfficeDto GetRentalById(int id)
        {
            _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);
            var rentalOffice = _dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            var rentalDto = _mapper.Map<RentalOfficeDto>(rentalOffice);
            return rentalDto;

        }

        public void PutRentalById(RentalOfficeUpdateDto dto, int id)
        {
            _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.PUT);

            var rentalOffice = _dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            NullRentalCheck(rentalOffice!, id);

            rentalOffice!.Name = dto.Name;
            rentalOffice.Description = dto.Description;
            rentalOffice.Category = (RentalCategory)Enum.Parse(typeof(RentalCategory), dto.Category, true);
            rentalOffice.AcceptUnder23 = dto.AcceptUnder23;
            rentalOffice.ConntactEmail = dto.ConntactEmail;
            rentalOffice.ConntactNumber = dto.ConntactNumber;

            _dbContext.SaveChanges();

            _logHandler.LogAction(ILogHandler.ActionEnum.Updated, rentalOffice.Id);
        }

        
    }
}