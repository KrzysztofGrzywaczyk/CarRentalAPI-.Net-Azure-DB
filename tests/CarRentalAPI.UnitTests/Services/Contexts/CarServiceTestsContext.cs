using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPI.UnitTests.Services.Contexts
{
    public class CarServiceTestsContext : UnitTestsContextBase
    {
        public CarServiceTestsContext()
        {
            DbContext = RentalOfficeDbFactory.CreateInMemoryDatabase();
            Service = CreateServiceFromMocks();
        }

        public CarService Service { get; private set; }

        public RentalDbContext DbContext { get; private set; }

        public override Task InitializeAsync()
        {
            
            AddRentalOfficeToDatabase();

            return Task.CompletedTask;
        }

        public override Task DisposeAsync() => Task.CompletedTask;

        private void AddRentalOfficeToDatabase()
        {
            var rentalOffice = new RentalOffice
            {
                Name = "Sample Rental Office",
                Description = "This is a sample rental office",
                Category = RentalOffice.RentalCategory.Casual,
                AcceptUnder23 = true,
                ConntactEmail = "rental@example.com",
                ConntactNumber = "123-456-789",
                Address = new Address { City = "Sample City", Street = "Sample Street" }
            };

        DbContext.rentalOffices.Add(rentalOffice);
        DbContext.SaveChanges();
        }

        private CarService CreateServiceFromMocks()
        {
        var mockLogHandler = new Mock<ILogHandler>();
        var mockMapper = new Mock<IMapper>();
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockUserContextService = new Mock<IUserContextService>();

        mockMapper.Setup(m => m.Map<Car>(It.IsAny<CreateCarDto>())).Returns((CreateCarDto dto) =>
        {
            return new Car
            {
                PlateNumber = dto.PlateNumber,
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                Segment = dto.Segment
            };
        });

            return new(DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);
        }   
    }
}
