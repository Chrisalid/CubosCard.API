namespace CubosCard.Application.Interfaces.Services;

public interface IAuthTokenService
{
    Task<bool> ValidateTokenAsync(string token);
}