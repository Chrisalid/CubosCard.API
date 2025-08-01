using System.Linq.Expressions;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface IUnitOfWorkRepository
{
    Task Create<TEntity>(TEntity entity) where TEntity : class;
    Task Update<TEntity>(TEntity entity) where TEntity : class;
    Task<TEntity?> GetById<TEntity, TId>(TId id) where TEntity : class;
    Task Delete<TEntity>(TEntity entity) where TEntity : class;
}
