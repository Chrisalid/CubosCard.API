using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface ICardRepository : IUnitOfWorkRepository
{
    Task<ICollection<Card>> GetByAccountId(Guid accountId);
    Task<ICollection<Card>> GetByPagination(Guid personId, int pageSize, int pageIndex);
}
