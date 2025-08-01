using CubosCard.Domain.Entities;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface IAccountRepository : IUnitOfWorkRepository
{
    Task<decimal> GetBalance(Guid accountId);
    Task<ICollection<Account>> GetByPersonId(Guid personId);
}
