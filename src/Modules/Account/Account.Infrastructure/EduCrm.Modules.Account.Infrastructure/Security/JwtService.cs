using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.SharedKernel.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EduCrm.Modules.Account.Infrastructure.Security;

internal sealed class JwtService : IJwtService
{
    private readonly JwtOptions _options;

    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public const string ApplicationAdminClaim = "app_admin";

    public string GenerateToken(Guid userId, bool isApplicationAdmin)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (isApplicationAdmin)
            claims.Add(new Claim(ApplicationAdminClaim, "true"));

        var keyBytes = Encoding.UTF8.GetBytes(_options.SigningKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
