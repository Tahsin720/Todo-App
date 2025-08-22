using System.Linq.Expressions;

namespace TodoApp.Services.Interfaces;

public interface IGenericService<TEntity, TId> where TEntity : class
{
    Task<TEntity> GetByIdAsync(TId id);
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TId id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<ICollection<TEntity>> ListAsync();
    Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> AddRangeAsync(ICollection<TEntity> entities);
    Task<int> UpdateRangeAsync(ICollection<TEntity> entities);
}
