using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class ExternalAuthenticationRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), IExternalAuthenticationRepository
{
    public async Task<ExternalAuthentication> GetByType(CubosApiType cubosApiType)
    {
        return await _dbContext.Set<ExternalAuthentication>()
            .Where(_ => _.Type == cubosApiType)
            .FirstOrDefaultAsync();
    }
}
