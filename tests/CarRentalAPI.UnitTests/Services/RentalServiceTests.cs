
using CarRentalAPI.Models;
using CarRentalAPI.UnitTests.Services.Contexts;

namespace CarRentalAPI.UnitTests.Services;

public class RentalServiceTests : UnitTestsBase<RentalServiceTestsContext>
{
    [Fact]
    public void WhenCreateRentalCalledCorrectly_ShouldCreateRental()
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
        Context.DbContext.rentalOffices.FirstOrDefault()!.Name.Should().Be(Context.TestRentalOffice.Name);
        Context.DbContext.rentalOffices.FirstOrDefault()!.Description.Should().Be(Context.TestRentalOffice.Description);
        Context.DbContext.rentalOffices.FirstOrDefault()!.Category.Should().Be(Context.TestRentalOffice.Category);
        Context.DbContext.rentalOffices.FirstOrDefault()!.AcceptUnder23.Should().Be(Context.TestRentalOffice.AcceptUnder23);
        Context.DbContext.rentalOffices.FirstOrDefault()!.ConntactEmail.Should().Be(Context.TestRentalOffice.ConntactEmail);
        Context.DbContext.rentalOffices.FirstOrDefault()!.ConntactNumber.Should().Be(Context.TestRentalOffice.ConntactNumber);
        Context.DbContext.rentalOffices.FirstOrDefault()!.AddressID.Should().Be(Context.TestRentalOffice.Address.Id);
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
}
