using CubosCard.Domain.Entities;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<ICollection<Transaction>> GetByPagination(Guid accountId, int pageSize, int pageIndex);
}
