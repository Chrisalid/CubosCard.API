using CubosCard.Domain.Entities;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface IAuthTokenRepository : IUnitOfWorkRepository
{
    Task<AuthToken> GetByPersonId(Guid personId);
    Task<bool> IsTokenValidAsync(string token);
}
