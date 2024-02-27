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

        // setup Get mapping
        mockMapper.Setup(m => m.Map<PresentRentalOfficeDto>(It.IsAny<RentalOffice>())).Returns((RentalOffice rentalOffice) =>
        {
            return new PresentRentalOfficeDto
            {
                Id = rentalOffice.Id,
                Name = rentalOffice.Name,
                Description = rentalOffice.Description,
                Category = rentalOffice.Category.ToString(),
                AcceptUnder23 = rentalOffice.AcceptUnder23,
                City = rentalOffice.Address?.City,
                Street = rentalOffice.Address?.Street,
                PostalCode = rentalOffice.Address?.PostalCode
            };
        });

        // setup GetAll mapping
        mockMapper.Setup(m => m.Map<List<PresentRentalOfficeDto>>(It.IsAny<List<RentalOffice>>())).Returns((List<RentalOffice> dtoList) =>
        {
            return new List<PresentRentalOfficeDto>
            {
                new PresentRentalOfficeDto
                {
                Id = dtoList.First().Id,
                Name = dtoList.First().Name,
                Description = dtoList.First().Description,
                Category = dtoList.First().Category.ToString(),
                AcceptUnder23 = dtoList.First().AcceptUnder23,
                City = dtoList.First().Address?.City,
                Street = dtoList.First().Address ?.Street,
                PostalCode = dtoList.First().Address ?.PostalCode
                }
            };
        });

        return mockMapper;
    }
}
