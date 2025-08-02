using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class TransactionRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), ITransactionRepository
{
    public async Task<ICollection<Transaction>> GetByPagination(Guid accountId, int pageSize, int pageIndex, TransactionType? type)
    {
        if (type is null)
            return await _dbContext.Set<Transaction>()
                .Include(t => t.Account)
                .Where(t => t.Account.Id == accountId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        return await _dbContext.Set<Transaction>()
                .Include(t => t.Account)
                .Where(t => t.Account.Id == accountId && t.Type == type)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
