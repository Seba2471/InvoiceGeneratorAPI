using FluentValidation;
using InvoiceGenerator.Requests;

namespace InvoiceGenerator.Validators
{
    public class SignInUserValidator : AbstractValidator<SignInUser>
    {
        public SignInUserValidator()
        {
            RuleFor(s => s.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(s => s.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .EmailAddress()
                .WithMessage("Email is incorrect");
        }
    }
}
