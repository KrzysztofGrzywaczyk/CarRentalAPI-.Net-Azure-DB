using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Queries;
using CarRentalAPI.UnitTests;
using CarRentalAPI.UnitTests.Services.Contexts;

namespace CarRentalAPI.Tests
{
    public class CarServiceTests : UnitTestsBase<CarServiceTestsContext>
    {
        [Fact]
        public void WhenAnyMethodCalledWithIncorrectRentalId_ShouldReturnNotFoundException()
        {
            // arrange
            var incorrectRentalId = 999;
            var dto = new CreateCarDto
            {
                PlateNumber = Context.TestCar.PlateNumber,
                Brand = Context.TestCar.Brand,
                Model = Context.TestCar.Model,
                Year = Context.TestCar.Year,
                Fuel = nameof(Context.TestCar.Fuel.Gas),
                Segment = Context.TestCar.Segment
            };

            // act
            var createAction = () => Context.Service.CreateCar(incorrectRentalId, dto);     
            var deleteAction = () => Context.Service.DeleteCar(incorrectRentalId, Context.TestCar.Id);            
            var getAllAction = () => Context.Service.GetAllCarsInRental(incorrectRentalId, new CarQuery());            
            var putAction = () => Context.Service.PutCar(incorrectRentalId, 2, dto);
            

            // assert
            createAction.Should().Throw<NotFoundException>();
            deleteAction.Should().Throw<NotFoundException>();
            getAllAction.Should().Throw<NotFoundException>();
            putAction.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void WhenAnyMethodCalledWithIncorrectCarId_ShouldReturnNotFoundException()
        {
            // arrange
            var incorrectCarId = 999;
            var dto = new CreateCarDto
            {
                PlateNumber = Context.TestCar.PlateNumber,
                Brand = Context.TestCar.Brand,
                Model = Context.TestCar.Model,
                Year = Context.TestCar.Year,
                Fuel = nameof(Context.TestCar.Fuel.Gas),
                Segment = Context.TestCar.Segment
            };

            // act
            var deleteAction = () => Context.Service.DeleteCar(Context.TestRentalOffice.Id, incorrectCarId);
            var getByIdAction = () => Context.Service.GetCarById(Context.TestRentalOffice.Id, incorrectCarId);
            var putCarAction = () => Context.Service.PutCar(Context.TestRentalOffice.Id, incorrectCarId, dto);

            // assert
            deleteAction.Should().Throw<NotFoundException>();
            getByIdAction.Should().Throw<NotFoundException>();
            putCarAction.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void WhenCreateCarCalledCorrectly_ShouldAddCarEntity()
        {         
            // arrange
            var createCarDto = new CreateCarDto
            {
                PlateNumber = Context.TestCar.PlateNumber,
                Brand = Context.TestCar.Brand,
                Model = Context.TestCar.Model,
                Year = Context.TestCar.Year,
                Fuel = nameof(Context.TestCar.Fuel.Gas),
                Segment = Context.TestCar.Segment
            };

            // act
            Context.Service.CreateCar(Context.TestRentalOffice.Id, createCarDto);

            // assert
            Context.DbContext.cars.Should().NotBeNullOrEmpty();
            Context.DbContext.cars.Count().Should().Be(1);
            Context.DbContext.cars.FirstOrDefault()!.PlateNumber.Should().Be(Context.TestCar.PlateNumber);
            Context.DbContext.cars.FirstOrDefault()!.Brand.Should().Be(Context.TestCar.Brand);
            Context.DbContext.cars.FirstOrDefault()!.Model.Should().Be(Context.TestCar.Model);
            Context.DbContext.cars.FirstOrDefault()!.Year.Should().Be(Context.TestCar.Year);
            Context.DbContext.cars.FirstOrDefault()!.Segment.Should().Be(Context.TestCar.Segment);
            Context.DbContext.cars.FirstOrDefault()!.RentalOfficeId.Should().Be(Context.TestRentalOffice.Id);
        }

        [Fact]
        public void WhenDeleteCarCalledCorrectly_ShouldDeleteExistingEntity()
        {
            // arrange
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            Context.Service.DeleteCar(Context.TestRentalOffice.Id, Context.TestCar.Id);

            // assert
            Context.DbContext.cars.Count().Should().Be(0);
            Context.DbContext.cars.Should().BeEmpty();
        }

        [Fact]
        public void WhenGetAllCarsInBaseCalledCorrectly_ShouldReturnSuccessfullyCars()
        {
            // arrange
            var query = new CarQuery();            
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            var result = Context.Service.GetAllCarsInBase(query);

            // assert
            result.Should().NotBeNull();
            result.TotalPages.Should().Be(1);
            result.Items?.Should().NotBeNull(); 
            result.Items!.Count().Should().Be(1);
        }

        [Fact]
        public void WhenGetAllCarsInRentalCalledCorrectly_ShouldReturnSuccesfullyCars()
        {
            // arrange
            var query = new CarQuery();
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            var result = Context.Service.GetAllCarsInRental(Context.TestRentalOffice.Id, query);

            // assert
            result.Should().NotBeNull();
            result.TotalPages.Should().Be(1);
            result.Items?.Should().NotBeNull();
            result.Items!.Count().Should().Be(1);
        }

        [Fact]
        public void WhenGetCarByIdCalledCorrectly_ShouldReturnSuccesfullyCar()
        {
            // arrange
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            var result = Context.Service.GetCarById(Context.TestRentalOffice.Id, Context.TestCar.Id);

            // assert
            result.Should().NotBeNull();
            result.Brand.Should().Be(Context.TestCar.Brand);
            result.Model.Should().Be(Context.TestCar.Model);
            result.Year.Should().Be(Context.TestCar.Year);
            result.Segment.Should().Be(Context.TestCar.Segment);
        }

        [Fact]
        public void WhenPutCarCalledCorrectly_ShouldSuccessfullyModifyExistingEntity()
        {
            // arrange
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            var modifiedCar = new CreateCarDto
            {
                PlateNumber = "NEW 123",
                Brand = "New brand",
                Model = "New model",
                Year = 2000,
                Fuel = nameof(Context.TestCar.Fuel.Gas),
                Segment = 'A'
            };

            // act
            Context.Service.PutCar(Context.TestRentalOffice.Id, Context.TestCar.Id, modifiedCar);

            // assert
            Context.DbContext.cars.Count().Should().Be(1);
            Context.DbContext.cars!.FirstOrDefault()!.PlateNumber.Should().Be("NEW 123");
            Context.DbContext.cars!.FirstOrDefault()!.Brand.Should().Be("New brand");
            Context.DbContext.cars!.FirstOrDefault()!.Model.Should().Be("New model");
            Context.DbContext.cars!.FirstOrDefault()!.Year.Should().Be(2000);
            Context.DbContext.cars!.FirstOrDefault()!.Segment.Should().Be('A');
            Context.DbContext.cars!.FirstOrDefault()!.RentalOfficeId.Should().Be(Context.TestRentalOffice.Id);
        }
    }
}
