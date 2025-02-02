using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repara.DTO.Auth;
using Repara.Model;

namespace Repara.Helpers;

public class JwtHandler
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly UserManager<User> _userManager;

    public JwtHandler(IOptions<JwtConfiguration> jwtConfiguration, UserManager<User> userManager)
    {
        _jwtConfiguration = jwtConfiguration.Value;
        _userManager = userManager;
    }

    public async Task<JwtSecurityToken> CreateToken(User user)
    {
        var authClaims = await GetClaims(user);
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret!));

        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private async Task<List<Claim>> GetClaims(User user)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Any())
        {
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        }

        return authClaims;
    }
}