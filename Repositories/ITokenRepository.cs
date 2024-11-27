using Microsoft.AspNetCore.Identity;

namespace NZWalks.Repositories
{
    public interface ITokenRepository
    {
       string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
