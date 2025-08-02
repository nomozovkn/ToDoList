using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoList.Bll.DTOs;

namespace ToDoList.Bll.Services.Helpers;
public class TokenService : ITokenService
{
    private IConfiguration _config;

    public TokenService(IConfiguration configuration)
    {
        _config = configuration.GetSection("Jwt");
    }
    public string GenerateToken(UserGetDto user)
    {
        var IdentityClaims = new Claim[]
        {
            new Claim("UserId",user.UserId.ToString()),
            new Claim("FirstName",user.FirstName.ToString()),
            new Claim("LastName",user.LastName.ToString()),
            new Claim("PhoneNumber",user.PhoneNumber.ToString()),
            new Claim("UserName",user.UserName.ToString()),
            new Claim(ClaimTypes.Role,user.Role.ToString()),
            new Claim(ClaimTypes.Email,user.Email.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityKey"]!));
        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresHours = int.Parse(_config["Lifetime"]);
        var token = new JwtSecurityToken(
            issuer: _config["Issuer"],
            audience: _config["Audience"],
            claims: IdentityClaims,
            expires: TimeHelper.GetDateTime().AddHours(expiresHours),
            signingCredentials: keyCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _config["Issuer"],
            ValidateAudience = true,
            ValidAudience = _config["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityKey"]!))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    }
}
