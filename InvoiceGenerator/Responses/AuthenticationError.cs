using Microsoft.AspNetCore.Identity;

namespace InvoiceGenerator.Responses
{
    public class AuthenticationError
    {
        public Dictionary<string, string[]> IdentityErrors { get; set; } = new Dictionary<string, string[]>();

        public AuthenticationError(List<IdentityError> identityErrors)
        {
            IdentityErrors = identityErrors
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
            IdentityErrors.Add(identityError.Code, new[] { identityError.Description });
        }

        public AuthenticationError(string key, string value)
        {
            IdentityErrors.Add(key, new[] { value });
        }
    }
}
