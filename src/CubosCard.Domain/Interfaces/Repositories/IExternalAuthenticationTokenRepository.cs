using System;
using CubosCard.Domain.Entities;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface IExternalAuthenticationTokenRepository : IUnitOfWorkRepository
{
    Task<ExternalAuthenticationToken> GetByExternalAuthenticationId(Guid externalAuthenticationId);
}
