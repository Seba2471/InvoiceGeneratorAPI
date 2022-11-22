using Microsoft.AspNetCore.Identity;

namespace InvoiceGenerator.Responses
{
    public class AuthenticationError
    {
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

        public AuthenticationError(List<IdentityError> identityErrors)
        {
            Errors = identityErrors
                .Where(x => x != null)
                .GroupBy(
                    x => x.Code,
                    x => x.Description,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);
        }

        public AuthenticationError(IdentityError identityError)
        {
            Errors.Add(identityError.Code, new[] { identityError.Description });
        }

        public AuthenticationError(string key, string value)
        {
            Errors.Add(key, new[] { value });
        }
    }
}
