using CarRentalAPI.Entities;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Models.Queries;
using FluentValidation;

namespace CarRentalAPI.Models.Validators
{
    public class RentalQueryValidator : AbstractValidator<RentalQuery>
    {
        private string[] allowedSortByColumnes =
        {
            nameof(RentalOffice.Name),
            nameof(RentalOffice.Category),
            nameof(RentalOffice.AcceptUnder23),
            nameof(RentalOffice.AddressID)
        };

        public RentalQueryValidator()
        {
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnes.Contains(value))
                .WithMessage($"Sort by is optional. If you want to use sorting options you need to use one of proper column names: ({string.Join(", ", allowedSortByColumnes)}) ");
        }
    }
}
