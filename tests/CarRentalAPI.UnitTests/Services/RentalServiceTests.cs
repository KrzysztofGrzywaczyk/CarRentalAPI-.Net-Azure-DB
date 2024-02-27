
using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Services;
using CarRentalAPI.UnitTests.Services.Contexts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.UnitTests.Services;

public class RentalServiceTests : UnitTestsBase<RentalServiceTestsContext>
{
    [Fact]
    public void WhenAnyNonRestrictedMethodCalledWithoutAuthentication_ShoulNotThrowForbidException()
    {
        // arrange
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockLogHandler = new Mock<ILogHandler>();
        var mockUserContextService = new Mock<IUserContextService>();
        var mockMapper = new Mock<IMapper>();

        mockAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Failed());

        var unauthenticatedService = new RentalService(Context.DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);

        Context.WithRentalOfficeCreatedInDatabase();

        var createDto = new CreateRentalOfficeDto();
        var query = new RentalQuery();

        // act
        var createAction = () => unauthenticatedService.CreateRental(createDto);
        var getAllAction = () => unauthenticatedService.GetRentalAll(query);
        var getByIdAction = () => unauthenticatedService.GetRentalById(Context.TestRentalOffice.Id);

        // assert
        createAction.Should().NotThrow<ForbidException>();
        getAllAction.Should().NotThrow<ForbidException>();
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

        var unauthenticatedService = new RentalService(Context.DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);

        Context.WithRentalOfficeCreatedInDatabase();

        var putDto = new UpdateRentalOfficeDto();

        // act       
        var deleteAction = () => unauthenticatedService.DeleteRental(Context.TestRentalOffice.Id);
        var updateAction = () => unauthenticatedService.PutRentalById(putDto, Context.TestRentalOffice.Id);

        // assert
        deleteAction.Should().Throw<ForbidException>();
        updateAction.Should().Throw<ForbidException>();
        
    }

    [Fact]
    public void WhenAnyMethodCalledWithIncorrectRentalId_ShouldThrowNotFountException()
    {
        // arrange
        var incorrectRentalId = 999;
        var rentalDto = new UpdateRentalOfficeDto();

        // act
        var deleteAction = () => Context.Service.DeleteRental(incorrectRentalId);
        var getByIdAction = () => Context.Service.GetRentalById(incorrectRentalId);
        var putAction = () => Context.Service.PutRentalById(rentalDto, incorrectRentalId);

        // assert
        deleteAction.Should().Throw<NotFoundException>();
        getByIdAction.Should().Throw<NotFoundException>();
        putAction.Should().Throw<NotFoundException>(); 

    }

    [Fact]
    public void WhenCreateRentalCalledCorrectly_ShouldAddRentalEntity()
    {
        // arrange
        var rentalDto = new CreateRentalOfficeDto
        {
            Name = Context.TestRentalOffice.Name,
            Description = Context.TestRentalOffice.Description,
            Category = nameof(Context.TestRentalOffice.Category),
            AcceptUnder23 = Context.TestRentalOffice.AcceptUnder23,
            ConntactEmail = Context.TestRentalOffice.ConntactEmail,
            ConntactNumber = Context.TestRentalOffice.ConntactNumber,
            City = Context.TestRentalOffice!.Address!.City,
            Street = Context.TestRentalOffice!.Address!.Street,
            PostalCode = Context.TestRentalOffice!.Address.PostalCode
        };

        // act
        Context.Service.CreateRental(rentalDto);

        // assert
        Context.DbContext.rentalOffices.Should().NotBeNullOrEmpty();
        Context.DbContext.rentalOffices.Count().Should().Be(1);
        Context.DbContext.rentalOffices.First().Should().NotBeNull();
        var entity = Context.DbContext.rentalOffices.First();
        
        entity.Should().BeOfType<RentalOffice>();
        entity!.Name.Should().Be(Context.TestRentalOffice.Name);
        entity.Description.Should().Be(Context.TestRentalOffice.Description);
        entity.Category.Should().Be(Context.TestRentalOffice.Category);
        entity.AcceptUnder23.Should().Be(Context.TestRentalOffice.AcceptUnder23);
        entity.ConntactEmail.Should().Be(Context.TestRentalOffice.ConntactEmail);
        entity.ConntactNumber.Should().Be(Context.TestRentalOffice.ConntactNumber);
        entity.AddressId.Should().Be(Context.TestRentalOffice.Address.Id);
    }

    [Fact]
    public void WhenDeleteRentalCalledCorrectly_ShouldDeleteExistingRental()
    {
        // arrange
        Context.WithRentalOfficeCreatedInDatabase();

        // act
        Context.Service.DeleteRental(Context.TestRentalOffice.Id);

        // assert
        Context.DbContext.rentalOffices.Count().Should().Be(0);
        Context.DbContext.rentalOffices.Should().BeEmpty();

    }

    [Fact]
    public void WhenGetAllRentalCalledCorrectly_ShouldSuccessfullyReturnRentals()
    {
        // arrange
        var query = new RentalQuery();
        Context.WithRentalOfficeCreatedInDatabase();

        // act
        var result = Context.Service.GetRentalAll(query);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResult<PresentRentalOfficeDto>>();
        result.TotalPages.Should().Be(1);
        result.Items.Should().NotBeNullOrEmpty();
        result.ItemCount.Should().Be(1);
        result.Items!.Count().Should().Be(1);
    }

    [Fact]
    public void WhenGetRentalByIdCalledCorrectly_ShouldSuccessfullyReturnRental()
    {
        // arrange
        Context.WithRentalOfficeCreatedInDatabase();

        // act
        var result = Context.Service.GetRentalById(Context.TestRentalOffice.Id);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PresentRentalOfficeDto>();
        result.Name.Should().Be(Context.TestRentalOffice.Name);
        result.Description.Should().Be(Context.TestRentalOffice.Description);
        result.Category.Should().Be(Context.TestRentalOffice.Category.ToString());
        result.AcceptUnder23.Should().Be(Context.TestRentalOffice.AcceptUnder23);
        result.City.Should().Be(Context?.TestRentalOffice?.Address?.City);
        result.Street.Should().Be(Context?.TestRentalOffice?.Address?.Street);
        result.PostalCode.Should().Be(Context?.TestRentalOffice?.Address?.PostalCode);
    }

    [Fact]
    public void WhenPutRentalOfficeCalledCorrectly_ShouldSuccessfullyModifyExistingEntity()
    {
        // arrange
        var rentalDto = new UpdateRentalOfficeDto
        {
            Name = "New Name",
            Description = "New Description",
            Category = nameof(RentalOffice.Category.Casual),
            AcceptUnder23 = true,
            ConntactEmail = "new@contact.email",
            ConntactNumber = Context.TestRentalOffice.ConntactNumber,
            City = "New City",
            Street = "New Street",
            PostalCode = "00-000"
        };

        Context.WithRentalOfficeCreatedInDatabase();

        // act
        Context.Service.PutRentalById(rentalDto, Context.TestRentalOffice.Id);

        // assert
        Context.DbContext.rentalOffices.First().Should().NotBeNull();
        Context.DbContext.rentalOffices.Count().Should().Be(1);
        var modifiedRental = Context.DbContext.rentalOffices.First();

        modifiedRental.Should().BeOfType<RentalOffice>();
        modifiedRental!.Name.Should().Be(rentalDto.Name);
        modifiedRental.Description.Should().Be(rentalDto.Description);
        modifiedRental.Category.ToString().Should().Be(rentalDto.Category);
        modifiedRental.AcceptUnder23.Should().Be(rentalDto.AcceptUnder23);
        modifiedRental.ConntactNumber.Should().Be(rentalDto.ConntactNumber);
        modifiedRental.Address?.Should().NotBeNull();
        modifiedRental.Address!.City.Should().Be(rentalDto.City);
        modifiedRental.Address.Street.Should().Be(rentalDto.Street);
        modifiedRental.Address.PostalCode.Should().Be(rentalDto.PostalCode);
    }
}
