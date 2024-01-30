using CarRentalAPI.Entities;
using CarRentalAPI.Models.Queries;
using FluentValidation;

namespace CarRentalAPI.Models.Validators
{
    public class CarQueryValidator : AbstractValidator<CarQuery>
    {
        private string[] allowedSortByColumnes =
        {
            nameof(Car.Brand),
            nameof(Car.Model),
            nameof(Car.Year),
            nameof(Car.Fuel),
            nameof(Car.Segment),
            nameof(Car.RentalOfficeId)
        };
       
        public CarQueryValidator()
        {
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnes.Contains(value))
                .WithMessage($"Sort by is optional. If you want to use sorting options you need to use one of proper column names: ({string.Join(", ",allowedSortByColumnes)}) ");
        }
    }
}
