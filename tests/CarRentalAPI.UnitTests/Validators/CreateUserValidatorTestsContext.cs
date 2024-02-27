using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Validators;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.UnitTests.Validators;

public class CreateUserValidatorTestsContext : UnitTestsContextBase
{
    private RentalDbContext dbContext;
    public CreateUserValidatorTestsContext()
    {
        dbContext = RentalOfficeDbFactory.CreateInMemoryDatabase();
        Validator = new(dbContext);
    }
    public CreateUserValidator Validator { get; }

    public readonly string testEmailAddress = "test@email.address";
    public readonly string testPassword = "password";

    public void WithAnAlreadyExistingUserWithEmailAddres()
    {
        var user = new User
        {
            Email = testEmailAddress
        };

        dbContext.users.Add(user);
        dbContext.SaveChanges();
    }
}

