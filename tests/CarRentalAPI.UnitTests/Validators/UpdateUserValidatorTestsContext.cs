using CarRentalAPI.Entities;
using CarRentalAPI.Models.Validators;

namespace CarRentalAPI.UnitTests.Validators;

public class UpdateUserValidatorTestsContext : UnitTestsContextBase
{
    public UpdateUserValidator Validator { get; } = new();
}
