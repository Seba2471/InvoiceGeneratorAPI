using FluentValidation.Results;

namespace InvoiceGenerator.Responses
{
    public class NotValidateRequest
    {
        public Dictionary<string, string[]> ValidationErrors { get; set; }
        public NotValidateRequest(List<ValidationFailure> validationErrors)
        {
            ValidationErrors = validationErrors
                .Where(x => x != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);
        }
    }
}
