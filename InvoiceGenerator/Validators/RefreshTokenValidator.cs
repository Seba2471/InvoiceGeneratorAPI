using FluentValidation;
using InvoiceGenerator.Requests;

namespace InvoiceGenerator.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshToken>
    {
        public RefreshTokenValidator()
        {

            RuleFor(t => t.Token)
                .NotEmpty()
                .NotNull();
        }
    }
}
