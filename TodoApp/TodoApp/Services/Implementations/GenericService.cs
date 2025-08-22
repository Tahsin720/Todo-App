using System.Linq.Expressions;
using TodoApp.DataAccess.Interfaces;
using TodoApp.Services.Interfaces;
using TodoApp.Utiities.Exceptions;

namespace TodoApp.Services.Implementations;

public class GenericService<TEntity, TId> : IGenericService<TEntity, TId> where TEntity : class
{
    private readonly IGenericRepository<TEntity, TId> _repository;

    public GenericService(IGenericRepository<TEntity, TId> repository)
    {
        _repository = repository;
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        return await _repository.AddAsync(entity);
    }

    public async Task<int> AddRangeAsync(ICollection<TEntity> entities)
    {
        return await _repository.AddRangeAsync(entities);
    }
    public async Task<int> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        return await _repository.UpdateRangeAsync(entities);
    }
    public async Task<bool> DeleteAsync(TId id)
    {
        TEntity entity = await _repository.GetByIdAsync(id) ?? throw new NotFoundException($"Entity of type {typeof(TEntity)} with id {id} not found");
        return await _repository.DeleteAsync(entity);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.GetAsync(predicate);
    }


    public virtual async Task<TEntity> GetByIdAsync(TId id)
    {
        TEntity? entity = await _repository.GetByIdAsync(id);
        return entity ?? throw new NotFoundException($"Entity of type {typeof(TEntity)} with id {id} not found");
    }

    public async Task<ICollection<TEntity>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    public Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _repository.ListAsync(predicate);
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        return await _repository.UpdateAsync(entity);
    }
}
