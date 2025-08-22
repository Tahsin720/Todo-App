using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Domain.Entities;
using TodoApp.Models.Auth;

namespace TodoApp.Services.Implementations.Token;
// Though Dot Core 8 Provides TokenService for JWT token generation and validation.
public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    public TokenService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public string? GetUserNameFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        JwtSecurityToken jwtToken = new(token);
        return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value;
    }

    public async Task<TokenModel> GetTokens(User user)
    {
        TokenModel accessTokenData = GenerateAccessToken(user);
        TokenModel refreshTokenData = await GenerateRefreshToken(user);
        return new TokenModel
        {
            AccessToken = accessTokenData.AccessToken,
            AccessTokenExpiresAt = accessTokenData.AccessTokenExpiresAt,
            RefreshToken = refreshTokenData.RefreshToken,
            RefreshTokenExpiresAt = refreshTokenData.RefreshTokenExpiresAt
        };
    }

    private TokenModel GenerateAccessToken(User user)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["Tokens:Key"]!));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expiresAt = DateTime.Now.AddDays(7);

        List<Claim> claims = GetAllValidClaims(user);

        JwtSecurityToken token = new(
            issuer: _config["Tokens:Issuer"],
            audience: _config["Tokens:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );
        string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new TokenModel
        {
            AccessToken = generatedToken,
            AccessTokenExpiresAt = expiresAt,
        };
    }
    private List<Claim> GetAllValidClaims(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Sid, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName?? "Todo User"),

        };
        return claims;
    }
    private async Task<TokenModel> GenerateRefreshToken(User user)
    {
        const int length = 80;
        Random random = new();
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        string token = new(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        DateTime expiresAt = DateTime.Now.AddDays(30);


        user.RefreshToken = token;
        user.RefreshTokenExpiresAt = expiresAt;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("User update failed");
        }

        return new TokenModel
        {
            RefreshToken = token,
            RefreshTokenExpiresAt = expiresAt
        };
    }
}
