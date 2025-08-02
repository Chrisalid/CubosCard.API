using CubosCard.Domain.Entities;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class ExternalAuthenticationTokenRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), IExternalAuthenticationTokenRepository
{
    public async Task<ExternalAuthenticationToken> GetByExternalAuthenticationId(Guid externalAuthenticationId)
    {
        return await _dbContext.Set<ExternalAuthenticationToken>()
            .Include(x => x.ExternalAuthentication)
            .Where(x => x.ExternalAuthentication.Id == externalAuthenticationId)
            .LastOrDefaultAsync();
    }
}
