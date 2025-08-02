using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface IExternalAuthenticationRepository : IUnitOfWorkRepository
{
    Task<ExternalAuthentication> GetByType(CubosApiType cubosApiType);
}
