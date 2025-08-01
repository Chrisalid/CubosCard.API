using CubosCard.Domain.Entities;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class AccountRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), IAccountRepository
{
    public async Task<ICollection<Account>> GetByPersonId(Guid personId)
    {
        return await _dbContext.Set<Account>()
            .Where(_ => _.PersonId == personId)
                .ToListAsync();
    }

    public async Task<decimal> GetBalance(Guid accountId)
    {
        var account = await _dbContext.Set<Account>().FindAsync(accountId) ?? throw new KeyNotFoundException($"Account with ID {accountId} not found.");

        return account.Amount;
    }
}
