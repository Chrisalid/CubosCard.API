using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface ITransactionRepository : IUnitOfWorkRepository
{
    Task<ICollection<Transaction>> GetByPagination(Guid accountId, int pageSize, int pageIndex, TransactionType? type);
}
