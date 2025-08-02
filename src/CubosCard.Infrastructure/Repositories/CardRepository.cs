using CubosCard.Domain.DTOs;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class CardRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), ICardRepository
{
    public async Task<ICollection<Card>> GetByAccountId(Guid accountId)
    {
        return await _dbContext.Set<Card>()
            .Where(_ => _.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<Card?> GetByAccountAndType(Guid accountId, CardType cardType)
    {
        return await _dbContext.Set<Card>()
            .Where(_ => _.AccountId == accountId && _.CardType == cardType)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<Card>> GetByPagination(Guid personId, int pageSize, int pageIndex)
    {
        var query = _dbContext.Set<Card>()
            .Include(t => t.Account)
            .Where(_ => _.Account.PersonId == personId);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var pagedResults = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return  new PagedResult<Card>
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = pagedResults
        };
    }
}
