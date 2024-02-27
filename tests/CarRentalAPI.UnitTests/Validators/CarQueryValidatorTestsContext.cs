using CarRentalAPI.Models.Validators;

namespace CarRentalAPI.UnitTests.Validators;

public class CarQueryValidatorTestsContext : UnitTestsContextBase
{
    public CarQueryValidator Validator { get; } = new(); 


}
