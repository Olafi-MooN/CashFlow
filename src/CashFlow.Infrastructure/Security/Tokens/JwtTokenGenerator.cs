using CashFlow.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationInMinutes;
    private readonly string _singingKey;

    public JwtTokenGenerator(uint expirationInMinutes, string signingKey)
    {
        _expirationInMinutes = expirationInMinutes;
        _singingKey = signingKey;
    }
    public string Generate(User user)
    {
        var claims = new List<Claim> {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Sid, user.UserIdentifier.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = System.Text.Encoding.UTF8.GetBytes(_singingKey);
        return new SymmetricSecurityKey(key);
    }
}
