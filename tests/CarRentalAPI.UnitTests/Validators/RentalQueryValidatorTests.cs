using CarRentalAPI.Entities;
using CarRentalAPI.Models.Pagination;
using FluentValidation.TestHelper;

namespace CarRentalAPI.UnitTests.Validators;

public class RentalQueryValidatorTests : UnitTestsBase<RentalQueryValidatorTestsContext>
{
    [Fact]
    public void WhenEmptyQueryIsCreated_ShouldHaveCorrectDefaultValues()
    {
        var defaultPageNumber = 1;
        var defaultPageSize = 20;
        var query = new RentalQuery();

        query.PageNumber.Should().Be(defaultPageNumber);
        query.PageSize.Should().Be(defaultPageSize);
    }

    [Fact]
    public void WhenSearchPhraseIsEmpty_ShouldValidateQueryCorrectly()
    {
        var query = new RentalQuery
        {
            SearchPhrase = string.Empty
        };

        Context.Validator
            .TestValidate(query)
            .ShouldNotHaveValidationErrorFor(x => x.SearchPhrase);
    }

    [Theory]
    [InlineData("Not valid name")]
    [InlineData("InvalidName")]
    [InlineData(" ")]
    public void WhenSortByColumnNamesAreInvalid_ShouldHaveValidationError(string columnName)
    {
        var query = new RentalQuery
        {
            SortBy = columnName
        };

        Context.Validator
            .TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.SortBy);
    }

    [Theory]
    [InlineData(nameof(RentalOffice.Name))]
    [InlineData(nameof(RentalOffice.Category))]
    [InlineData(nameof(RentalOffice.AcceptUnder23))]
    [InlineData(nameof(RentalOffice.AddressId))]
    [InlineData(nameof(RentalOffice.OwnerId))]
    
    public void WhenSortByColumnNamesAreValid_ShouldValidateQueryCorrectly(string columnName)
    {
        var query = new RentalQuery
        {
            SortBy = columnName
        };

        Context.Validator
            .TestValidate(query)
            .ShouldNotHaveValidationErrorFor(x => x.SortBy);
    }
}
