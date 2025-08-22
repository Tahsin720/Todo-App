using System.Linq.Expressions;
namespace TodoApp.DataAccess.Interfaces;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task<ICollection<TEntity>> ListAsync();
    Task<bool> AddAsync(TEntity entity);
    Task<int> AddRangeAsync(ICollection<TEntity> entities);
    Task<bool> UpdateAsync(TEntity entity);
    Task<int> UpdateRangeAsync(ICollection<TEntity> entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
}
