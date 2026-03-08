using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace pingword.src.Interfaces.Users
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
