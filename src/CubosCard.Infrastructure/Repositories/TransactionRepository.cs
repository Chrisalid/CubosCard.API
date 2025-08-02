using CubosCard.Domain.DTOs;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class TransactionRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), ITransactionRepository
{
    public async Task<PagedResult<Transaction>> GetByPagination(Guid accountId, int pageSize, int pageIndex, TransactionType? type)
    {
        var query = _dbContext.Set<Transaction>()
            .Include(t => t.Account)
            .Where(t => t.Account.Id == accountId && (type == null || t.Type == type));

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var pagedResults = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Transaction>
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = pagedResults
        };
    }
}
