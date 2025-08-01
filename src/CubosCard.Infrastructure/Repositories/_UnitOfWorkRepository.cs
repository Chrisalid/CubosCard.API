using System;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class UnitOfWorkRepository(ApplicationDbContext context) : IUnitOfWorkRepository
{
    protected readonly ApplicationDbContext _dbContext = context;

    private DbSet<TEntity> BuildDataSet<TEntity>() where TEntity : class
    {
        return _dbContext.Set<TEntity>();
    }

    public async Task Create<TEntity>(TEntity entity) where TEntity : class
    {
        var dbSet = BuildDataSet<TEntity>();

        await dbSet.AddAsync(entity).ConfigureAwait(false);

        await SaveChanges();
    }

    public async Task Update<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Entry(entity).State = EntityState.Modified;

        await SaveChanges();
    }

    public async Task<TEntity> GetById<TEntity, TId>(TId id) where TEntity : class
    {
        var dbSet = BuildDataSet<TEntity>();
        return await dbSet.FindAsync(id);
    }

    public async Task Delete<TEntity>(TEntity entity) where TEntity : class
    {
        var dbSet = BuildDataSet<TEntity>();
        dbSet.Remove(entity);
        await SaveChanges();
    }

    private async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
