using Microsoft.AspNetCore.Identity;

namespace JobAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
