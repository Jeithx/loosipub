using System.Linq.Expressions;

namespace Core.DataAccess;

public interface IEntityRepositoryAsync<TEntity> where TEntity : class, new()
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, string[]? includes = null);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null);
    Task<bool> UpdateAsync(TEntity entity);
    Task<TEntity?> FindAsync(int id);

    Task<List<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null,
        int pageNumber = 1, int pageSize = 10);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null);

    Task<List<TEntity>> GetPageAsync<TKey>(Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TKey>>? orderByDescending = null,
        Expression<Func<TEntity, TKey>>? orderBy = null, string[]? includes = null, int pageNumber = 1,
        int pageSize = 10,
        Expression<Func<TEntity, TKey>>? groupBy = null);
}