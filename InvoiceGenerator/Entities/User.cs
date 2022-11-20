using Microsoft.AspNetCore.Identity;

namespace InvoiceGenerator.Entities
{
    public class User : IdentityUser
    {
        public string Email { get; set; }
    }
}
