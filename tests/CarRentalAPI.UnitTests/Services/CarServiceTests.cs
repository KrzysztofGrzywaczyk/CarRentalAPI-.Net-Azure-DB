
using CarRentalAPI.Models;
using CarRentalAPI.UnitTests;
using CarRentalAPI.UnitTests.Services.Contexts;

namespace CarRentalAPI.Tests
{
    public class CarServiceTests : UnitTestsBase<CarServiceTestsContext>
    {

        [Fact]
        public void WhenCreateCarCalled_ShouldCreateCar()
        {
            // arrange
            var createCarDto = new CreateCarDto
            {
                PlateNumber = "ABC123",
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2022,
                Fuel = "Gas",
                Segment = 'E'
            };

            // act
            Context.Service.CreateCar(1, createCarDto);

            // assert
            Context.DbContext.cars.Count().Should().Be(1);
            Context.DbContext.cars!.FirstOrDefault()!.PlateNumber.Should().Be("ABC123");
            Context.DbContext.cars!.FirstOrDefault()!.Brand.Should().Be("Toyota");
            Context.DbContext.cars!.FirstOrDefault()!.Model.Should().Be("Corolla");
            Context.DbContext.cars!.FirstOrDefault()!.Year.Should().Be(2022);
            Context.DbContext.cars!.FirstOrDefault()!.Segment.Should().Be('E');
            Context.DbContext.cars!.FirstOrDefault()!.Id.Should().Be(1);
            Context.DbContext.cars!.FirstOrDefault()!.RentalOfficeId.Should().Be(1);
        }
    }
}