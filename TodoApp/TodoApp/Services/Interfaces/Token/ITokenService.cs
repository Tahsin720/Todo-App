using TodoApp.Domain.Entities;
using TodoApp.Models.Auth;

public interface ITokenService
{
    Task<TokenModel> GetTokens(User user);
    string? GetUserNameFromToken(string token);
}
