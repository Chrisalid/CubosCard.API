using CubosCard.Domain.Entities;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class AuthTokenRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), IAuthTokenRepository
{
    public async Task<AuthToken> GetByPersonId(Guid personId)
    {
        return await _dbContext.Set<AuthToken>()
            .Where(_ => _.PersonId == personId && _.ExpiresAt <= DateTime.Now)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        var authToken = await _dbContext.AuthToken
            .FirstOrDefaultAsync(t => t.Token == token);

        if (authToken == null || authToken.ExpiresAt < DateTime.UtcNow)
            return false;

        return true;
    }
}
