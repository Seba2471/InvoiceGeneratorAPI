using InvoiceGenerator.Entities;
using Microsoft.AspNetCore.Identity;

namespace InvoiceGenerator.Persistence
{
    public interface ITokenRepository<T> : IAsyncRepository<RefreshToken> where T : IdentityUser
    {
        string GenerateAccessToken(T user, IList<string> roles);
        RefreshToken GenereateRefreshToken(T user);
        Task<RefreshToken> GetByTokenValue(string refreshToken);
        bool ValidateRefreshToken(string refreshToken);
    }
}
