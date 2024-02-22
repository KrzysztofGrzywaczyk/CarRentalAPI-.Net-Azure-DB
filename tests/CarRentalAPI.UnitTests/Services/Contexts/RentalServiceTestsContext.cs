
using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.UnitTests.Services.Contexts;

public class RentalServiceTestsContext : UnitTestsContextBase
{
    public RentalServiceTestsContext()
    {
        DbContext = RentalOfficeDbFactory.CreateInMemoryDatabase();
        Service = CreateServiceFromMocks();
    }

    public RentalDbContext DbContext { get; private set; }

    public RentalService Service { get; private set; }


    public RentalOffice TestRentalOffice { get; private set; } = new()
    {
        Id = 1,
        Name = "Sample Rental Office",
        Description = "This is a sample rental office",
        Category = RentalOffice.RentalCategory.Casual,
        AcceptUnder23 = true,
        ConntactEmail = "rental@example.com",
        ConntactNumber = "123-456-789",
        Address = new Address { Id = 1, City = "Sample City", Street = "Sample Street", PostalCode = "55-555" }
    };

    public override Task InitializeAsync()
    {
        return base.InitializeAsync();
    }

    public override Task DisposeAsync()
    {
        return base.DisposeAsync();
    }

    private RentalService CreateServiceFromMocks()
    {
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockLogHandler = new Mock<ILogHandler>();
        var mockUserContextService = new Mock<IUserContextService>();
        var mockMapper = SetupMockMapper();

        mockAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Success());

        return new RentalService(DbContext, mockLogHandler.Object, mockMapper.Object, mockAuthorizationService.Object, mockUserContextService.Object);
    }

    public void WithRentalOfficeCreatedInDatabase()
    {
        var rentalDto = new CreateRentalOfficeDto
        {
            Name = TestRentalOffice.Name,
            Description = TestRentalOffice.Description,
            Category = nameof(TestRentalOffice.Category),
            AcceptUnder23 = TestRentalOffice.AcceptUnder23,
            ConntactEmail = TestRentalOffice.ConntactEmail,
            ConntactNumber = TestRentalOffice.ConntactNumber,
            City = TestRentalOffice!.Address!.City,
            Street = TestRentalOffice!.Address!.Street,
            PostalCode = TestRentalOffice!.Address.PostalCode
        };

        Service.CreateRental(rentalDto);
    }

    private Mock<IMapper> SetupMockMapper()
    {
        var mockMapper = new Mock<IMapper>();

        // setup Create mapping
        mockMapper.Setup(m => m.Map<RentalOffice>(It.IsAny<CreateRentalOfficeDto>())).Returns((CreateRentalOfficeDto dto) =>
        {
            return new RentalOffice
            {
                Name = dto.Name,
                Description = dto.Description,
                AcceptUnder23 = dto.AcceptUnder23,
                ConntactEmail = dto.ConntactEmail,
                ConntactNumber = dto.ConntactNumber,
                Address = new Address { City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }
            };
        });



        return mockMapper;
    }
}
