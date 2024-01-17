using CarRentalAPI.Entities;
using FluentValidation;

namespace CarRentalAPI.Models.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator(RentalDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);

            RuleFor(x => x.PasswordConfirmation).Equal(e => e.Password);

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var isEmailUsed = dbContext.users.Any(u => u.Email == value);
                if (isEmailUsed) 
                {
                    context.AddFailure("Email", "That email is already in use.");
                }
            });
        }
    }
}
