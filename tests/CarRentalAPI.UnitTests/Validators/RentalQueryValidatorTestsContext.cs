using CarRentalAPI.Models.Validators;

namespace CarRentalAPI.UnitTests.Validators;

public class RentalQueryValidatorTestsContext : UnitTestsContextBase
{
    public RentalQueryValidator Validator { get; } = new();
}
