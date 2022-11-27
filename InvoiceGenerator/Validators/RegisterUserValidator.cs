using FluentValidation;
using InvoiceGenerator.Requests;

namespace InvoiceGenerator.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Bad email address")
                .NotNull()
                .WithMessage("{PropertyName} is required");

            RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match");
        }
    }
}
