using CarRentalAPI.Entities;
using FluentValidation;

namespace CarRentalAPI.Models.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator(RentalDbContext dbContext)
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).MinimumLength(6);

        RuleFor(x => x.PasswordConfirmation).Equal(e => e.Password);
    }
}