using FluentValidation;
using InvoiceGenerator.Requests;

namespace InvoiceGenerator.Validators
{
    public class PaginationValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationValidator()
        {
            RuleFor(q => q.PageSize)
                .GreaterThan(0)
                .NotNull()
                .NotEmpty();

            RuleFor(q => q.PageNumber)
                .GreaterThan(0)
                .NotNull()
                .NotEmpty();
        }
    }
}
