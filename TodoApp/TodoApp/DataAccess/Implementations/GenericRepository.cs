using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoApp.DataAccess.Interfaces;
using TodoApp.Domain.Database;

namespace TodoApp.DataAccess.Implementations;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
{
    private readonly AppDbContext _dbContext;

    private DbSet<TEntity> _dbSet => _dbContext.Set<TEntity>();
    private readonly Type entityType = typeof(TEntity);

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity).AsTask();
        return await _dbContext.SaveChangesAsync() > 0;

    }

    public async Task<int> AddRangeAsync(ICollection<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<ICollection<TEntity>> ListAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<int> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        return await _dbContext.SaveChangesAsync();
    }
}
