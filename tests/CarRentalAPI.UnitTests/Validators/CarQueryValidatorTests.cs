using CarRentalAPI.Entities;
using CarRentalAPI.Models.Queries;
using FluentValidation.TestHelper;

namespace CarRentalAPI.UnitTests.Validators;

public class CarQueryValidatorTests : UnitTestsBase<CarQueryValidatorTestsContext>
{
    [Fact]
    public void WhenEmptyQueryIsCreated_ShouldHaveCorrectDefaultValues()
    {
        var defaultPageNumber = 1;
        var defaultPageSize = 20;
        var query = new CarQuery();

        query.PageNumber.Should().Be(defaultPageNumber);
        query.PageSize.Should().Be(defaultPageSize);   
    } 

    [Fact]
    public void WhenSearchPhraseIsEmpty_ShouldValidateQueryCorrectly()
    {
        var query = new CarQuery
        {
            SearchPhrase = string.Empty
        };

        Context.Validator
            .TestValidate(query)
            .ShouldNotHaveValidationErrorFor(q  => q.SearchPhrase);
    }

    [Theory]
    [InlineData("Not valid name")]
    [InlineData("InvalidName")]
    [InlineData(" ")]
    public void WhenSortByColumnNamesAreInvalid_ShouldHaveValidationError(string columnName)
    {
        var query = new CarQuery
        {
            SortBy = columnName
        };

        Context.Validator
            .TestValidate(query)
            .ShouldHaveValidationErrorFor(q => q.SortBy);
    }

    [Theory]
    [InlineData(nameof(Car.Brand))]
    [InlineData(nameof(Car.Model))]
    [InlineData(nameof(Car.Year))]
    [InlineData(nameof(Car.Fuel))]
    [InlineData(nameof(Car.Segment))]

    public void WhenSortByColumnNamesAreValid_ShouldValidateQueryCorrectly(string columnName)
    {
        var query = new CarQuery
        {
            SortBy = columnName
        };

        Context.Validator
            .TestValidate(query)
            .ShouldNotHaveValidationErrorFor(q => q.SortBy);
    }
}
