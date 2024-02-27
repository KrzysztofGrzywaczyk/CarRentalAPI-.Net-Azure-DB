using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.UnitTests.Services.Contexts
{
    public class CarServiceTestsContext : UnitTestsContextBase
    {
        public CarServiceTestsContext()
        {
            DbContext = RentalOfficeDbFactory.CreateInMemoryDatabase();
            Service = CreateServiceFromMocks();
        }

        public Car TestCar { get; private set; } = new ()
            {
                Id = 1,
                PlateNumber = "ABC123",
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2022,
                Fuel = Car.FuelType.Gas,
                Segment = 'E'
            };

        public RentalOffice TestRentalOffice { get; private set; } = new ()
        {
            Id = 1,
            Name = "Sample Rental Office",
            Description = "This is a sample rental office",
            Category = RentalOffice.RentalCategory.Casual,
            AcceptUnder23 = true,
            ConntactEmail = "rental@example.com",
            ConntactNumber = "123-456-789",
            Address = new Address { City = "Sample City", Street = "Sample Street" }
        };

        public CarService Service { get; private set; }

        public RentalDbContext DbContext { get; private set; }

        public override async Task InitializeAsync()
        {
            AddRentalOfficeToDatabase();
            await base.InitializeAsync();
        }

        public override async Task DisposeAsync()
        {
            DbContext.Dispose();
            await base.DisposeAsync();
        }

        public void WithCarCreatedInRentalOffice(int testRentalOfficeId)
        {
            var car = new Car
            {
                PlateNumber = TestCar.PlateNumber,
                Brand = TestCar.Brand,
                Model = TestCar.Model,
                Year = TestCar.Year,
                Fuel = Car.FuelType.Gas,
                Segment = TestCar.Segment,
                RentalOfficeId = testRentalOfficeId
            };

            DbContext.cars.Add(car);
            DbContext.SaveChanges();
            
        }

        private void AddRentalOfficeToDatabase()
        {
            var rentalOffice = new RentalOffice
            {
                Name = TestRentalOffice.Name,
                Description = TestRentalOffice.Description,
                Category = TestRentalOffice.Category,
                AcceptUnder23 = TestRentalOffice.AcceptUnder23,
                ConntactEmail = TestRentalOffice.ConntactEmail,
                ConntactNumber = TestRentalOffice.ConntactNumber,
                Address = TestRentalOffice.Address
            };

            DbContext.rentalOffices.Add(rentalOffice);
            DbContext.SaveChanges();
        }

        private CarService CreateServiceFromMocks()
        {
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockLogHandler = new Mock<ILogHandler>();      
        var mockUserContextService = new Mock<IUserContextService>();
        var mockMapper = SetupMockMapper();

        mockAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Success());

        return new CarService(DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);
        }   

        private static Mock<IMapper> SetupMockMapper()
        {
            var mockMapper = new Mock<IMapper>();

            // setup Create mapping
            mockMapper.Setup(m => m.Map<Car>(It.IsAny<CreateUpdateCarDto>())).Returns((CreateUpdateCarDto dto) =>
            {
                return new Car
                {
                    PlateNumber = dto.PlateNumber,
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    Segment = dto.Segment,
                };
            });

            // setup Get mapping
            mockMapper.Setup(m => m.Map<PresentCarDto>(It.IsAny<Car>())).Returns((Car car) =>
            {
                return new PresentCarDto
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Year = car.Year,
                    Segment = car.Segment
                };
            });

            // setup GetAll mapping
            mockMapper.Setup(m => m.Map<List<PresentCarAllCarsDto>>(It.IsAny<List<Car>>())).Returns((List<Car> listOfCars) =>
            {
                return new List<PresentCarAllCarsDto>()
                {
                    new PresentCarAllCarsDto()
                    {
                        Id = listOfCars.First().Id,
                        Brand = listOfCars.First().Brand,
                        Model = listOfCars.First().Model,
                        Year = listOfCars.First().Year,
                        Segment = listOfCars.First().Segment,
                        RentalOfficeId = listOfCars.First().RentalOfficeId
                    }
                };
            });

            // setup GetAllInRental mapping
            mockMapper.Setup(m => m.Map<List<PresentCarDto>>(It.IsAny<List<Car>>())).Returns((List<Car> listOfCars) =>
            {
                return new List<PresentCarDto>
                {
                    new PresentCarDto
                    {
                        Id = listOfCars.First().Id,
                        Brand = listOfCars.First().Brand,
                        Model = listOfCars.First().Model,
                        Year = listOfCars.First().Year,
                        Segment = listOfCars.First().Segment,
                    }
                };
            });

            return mockMapper;
        }
    }
}
