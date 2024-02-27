using CarRentalAPI.Models;
using FluentValidation.TestHelper;

namespace CarRentalAPI.UnitTests.Validators;

public class UpdateUserValidatorTests : UnitTestsBase<UpdateUserValidatorTestsContext>
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Invalid#Format")]
    [InlineData("InvalidFormat.com")]
    public void WhenEmailIsEmptyOrInvailid_ShouldHaveValidationError(string email)
    {
        var createDto = new UpdateUserDto()
        {
            Email = email,
            Password = "password",
            PasswordConfirmation = "password",
        };

        Context.Validator
            .TestValidate(createDto)
                .ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("correct@email.address")]
    [InlineData("example@gmail.com")]

    public void WhenEmailIsCorrect_ShouldValidateCorrectly(string email)
    {
        var createDto = new UpdateUserDto()
        {
            Email = email,
            Password = "password",
            PasswordConfirmation = "password",
        };

        Context.Validator
            .TestValidate(createDto)
                .ShouldNotHaveValidationErrorFor(c => c.Email);
    }


    [Theory]
    [InlineData("password", "diferentpassword")]
    [InlineData("Test Password", "Other Password")]
    public void WhenPasswordAndConfirmationAreDiferent_ShouldHaveValidationError(string password, string confirmation)
    {
        var createDto = new UpdateUserDto()
        {
            Email = "test@email.com",
            Password = password,
            PasswordConfirmation = confirmation
        };

        Context.Validator.TestValidate(createDto).ShouldHaveValidationErrorFor(c => c.PasswordConfirmation);
    }

    [Theory]
    [InlineData("password", "password")]
    [InlineData("Test Password", "Test Password")]
    public void WhenPasswordAndConfirmationAreTheSame_ShouldNotHaveValidationError(string password, string confirmation)
    {
        var createDto = new UpdateUserDto()
        {
            Email = "test@email.com",
            Password = password,
            PasswordConfirmation = confirmation
        };

        Context.Validator.TestValidate(createDto).ShouldNotHaveValidationErrorFor(c => c.PasswordConfirmation);
    }


    [Theory]
    [InlineData("short")]
    [InlineData("test")]
    [InlineData("pass")]
    [InlineData("")]
    public void WhenPasswordIsEmptyOrTooShort_ShouldHaveValidationError(string password)
    {
        var createDto = new UpdateUserDto()
        {
            Email = "test@email.com",
            Password = password,
            PasswordConfirmation = password
        };

        Context.Validator.TestValidate(createDto).ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Theory]
    [InlineData("Long password")]
    [InlineData("123456")]
    [InlineData("LongPass")]
    public void WhenPasswordIsLongerThanMinimumValue_ShouldHaveNotValidationError(string password)
    {
        var createDto = new UpdateUserDto()
        {
            Email = "test@email.com",
            Password = password,
            PasswordConfirmation = password
        };

        Context.Validator.TestValidate(createDto).ShouldNotHaveValidationErrorFor(c => c.Password);
    }
}
