using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Models.Queries;
using CarRentalAPI.Services;
using CarRentalAPI.UnitTests.Services.Contexts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.UnitTests.Services
{
    public class CarServiceTests : UnitTestsBase<CarServiceTestsContext>
    {
        [Fact]
        public void WhenAnyNonRestrictedMethodCalledWithoutAuthentication_ShouldNotThrowForbidException()
        {
            // arrange
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            var mockLogHandler = new Mock<ILogHandler>();
            var mockUserContextService = new Mock<IUserContextService>();
            var mockMapper = new Mock<IMapper>();

            mockAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Failed());

            var unauthenticatedService = new CarService(Context.DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);

            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            var createDto = new CreateUpdateCarDto();
            var querry = new CarQuery();

            // act
            var createAction = () => unauthenticatedService.CreateCar(Context.TestRentalOffice.Id, createDto);
            var getAllInDbAction = () => unauthenticatedService.GetAllCarsInBase(querry);
            var getAllInRentalAction = () => unauthenticatedService.GetAllCarsInRental(Context.TestRentalOffice.Id, querry);
            var getByIdAction = () => unauthenticatedService.GetCarById(Context.TestRentalOffice.Id, Context.TestCar.Id);

            // assert
            createAction.Should().NotThrow<ForbidException>();
            getAllInDbAction.Should().NotThrow<ForbidException>();
            getAllInRentalAction.Should().NotThrow<ForbidException>();
            getByIdAction.Should().NotThrow<ForbidException>();
        }

        [Fact]
        public void WhenAnyRestrictedMethodCalledWithoutAuthentication_ShouldThrowForbidException()
        {
            // arrange
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            var mockLogHandler = new Mock<ILogHandler>();
            var mockUserContextService = new Mock<IUserContextService>();
            var mockMapper = new Mock<IMapper>();

            mockAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Failed());

            var unauthenticatedService = new CarService(Context.DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);

            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            var dto = new CreateUpdateCarDto();

            // act
            var deleteAction = () => unauthenticatedService.DeleteCar(Context.TestRentalOffice.Id, Context.TestCar.Id);
            var updateAction = () => unauthenticatedService.PutCar(Context.TestRentalOffice.Id, Context.TestCar.Id, dto);



        }

        [Fact]
        public void WhenAnyMethodCalledWithIncorrectRentalId_ShouldReturnNotFoundException()
        {
            // arrange
            var incorrectRentalId = 999;
            var dto = new CreateUpdateCarDto
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
            var getByIdAction = () => Context.Service.GetCarById(incorrectRentalId, Context.TestCar.Id);
            var putAction = () => Context.Service.PutCar(incorrectRentalId, 2, dto);
            

            // assert
            createAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.rentalNotFoundMessage);
            deleteAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.rentalNotFoundMessage);
            getAllAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.rentalNotFoundMessage);
            getByIdAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.rentalNotFoundMessage);
            putAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.rentalNotFoundMessage);
        }

        [Fact]
        public void WhenAnyMethodCalledWithIncorrectCarId_ShouldReturnNotFoundException()
        {
            // arrange
            var incorrectCarId = 999;
            var dto = new CreateUpdateCarDto
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
            var putAction = () => Context.Service.PutCar(Context.TestRentalOffice.Id, incorrectCarId, dto);

            // assert
            deleteAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.carNotFoundMessage);
            getByIdAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.carNotFoundMessage);
            putAction.Should().Throw<NotFoundException>()
                .WithMessage(Context.Service.carNotFoundMessage);
        }

        [Fact]
        public void WhenCreateCarCalledCorrectly_ShouldAddCarEntity()
        {         
            // arrange
            var createCarDto = new CreateUpdateCarDto
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
            Context.DbContext.cars.First().Should().NotBeNull();
            var entity = Context.DbContext.cars.First();

            entity.Should().BeOfType<Car>();
            entity.PlateNumber.Should().Be(Context.TestCar.PlateNumber);
            entity.Brand.Should().Be(Context.TestCar.Brand);
            entity.Model.Should().Be(Context.TestCar.Model);
            entity.Year.Should().Be(Context.TestCar.Year);
            entity.Segment.Should().Be(Context.TestCar.Segment);
            entity.RentalOfficeId.Should().Be(Context.TestRentalOffice.Id);
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
        public void WhenGetAllCarsInBaseCalledCorrectly_ShouldSuccessfullyReturnCars()
        {
            // arrange
            var query = new CarQuery();            
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            var result = Context.Service.GetAllCarsInBase(query);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PagedResult<PresentCarAllCarsDto>>();
            result.TotalPages.Should().Be(1);
            result.Items.Should().NotBeNullOrEmpty(); 
            result.ItemCount.Should().Be(1);
            result.Items!.Count().Should().Be(1);
        }

        [Fact]
        public void WhenGetAllCarsInRentalCalledCorrectly_ShouldSuccesfullyReturnCars()
        {
            // arrange
            var query = new CarQuery();
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            var result = Context.Service.GetAllCarsInRental(Context.TestRentalOffice.Id, query);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PagedResult<PresentCarDto>>();
            result.TotalPages.Should().Be(1);
            result.Items?.Should().NotBeNull();
            result.Items!.Count().Should().Be(1);
        }

        [Fact]
        public void WhenGetCarByIdCalledCorrectly_ShouldSuccesfullyReturnCar()
        {
            // arrange
            Context.WithCarCreatedInRentalOffice(Context.TestRentalOffice.Id);

            // act
            var result = Context.Service.GetCarById(Context.TestRentalOffice.Id, Context.TestCar.Id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PresentCarDto>();
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

            var carDto = new CreateUpdateCarDto
            {
                PlateNumber = "NEW 123",
                Brand = "New brand",
                Model = "New model",
                Year = 2000,
                Fuel = nameof(Context.TestCar.Fuel.Gas),
                Segment = 'A'
            };

            // act
            Context.Service.PutCar(Context.TestRentalOffice.Id, Context.TestCar.Id, carDto);

            // assert
            Context.DbContext.cars.First().Should().NotBeNull();
            Context.DbContext.cars.Count().Should().Be(1);
            var modifiedCar = Context.DbContext.cars.First();

            modifiedCar.Should().BeOfType<Car>();
            modifiedCar.PlateNumber.Should().Be("NEW 123");
            modifiedCar.Brand.Should().Be("New brand");
            modifiedCar.Model.Should().Be("New model");
            modifiedCar.Year.Should().Be(2000);
            modifiedCar.Segment.Should().Be('A');
            modifiedCar.RentalOfficeId.Should().Be(Context.TestRentalOffice.Id);
        }
    }
}
