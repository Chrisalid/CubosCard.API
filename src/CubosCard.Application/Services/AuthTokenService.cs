using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace CubosCard.Application.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly IAuthTokenRepository _authTokenRepository;

    public AuthTokenService(IAuthTokenRepository authTokenRepository, IConfiguration configuration)
    {
        _authTokenRepository = authTokenRepository;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try { return await _authTokenRepository.IsTokenValidAsync(token); }
        catch { throw; }
    }
}