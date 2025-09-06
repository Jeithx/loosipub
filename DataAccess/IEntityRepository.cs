using System.Linq.Expressions;

namespace Core.DataAccess;

public interface IEntityRepository<T> where T : class, new()
{
    T Get(Expression<Func<T, bool>>? filter, string[]? includes = null);
    T Find(int id);
    bool Any(Expression<Func<T, bool>> filter);
    int Count(Expression<Func<T, bool>>? filter = null, string[]? includes = null);

    #region Search

    IEnumerable<T> Search(Func<T, bool> predicate, Expression<Func<T, bool>>? filter = null, string[]? includes = null,
        int pageNumber = 1, int pageSize = 0);

    #endregion

    IEnumerable<T> GetFromEnd(Expression<Func<T, bool>>? filter = null, string[]? includes = null,
        Expression<Func<T, long>>? orderBy = null, int count = 10);

    IEnumerable<T> GetFromStart(Expression<Func<T, bool>>? filter = null, string[]? includes = null,
        Expression<Func<T, long>>? orderBy = null, int count = 10);

    IList<T> GetList(Expression<Func<T, bool>>? filter = null, string[]? includes = null);
    T Add(T entity);
    bool Update(T entity);
    bool SoftDelete(T entity);
    bool Delete(T entity);

    #region Pagination

    IEnumerable<T> GetPage(Expression<Func<T, bool>>? filter = null, string[]? includes = null, int pageNumber = 1,
        int pageSize = 0);

    int GetTotalCount(Expression<Func<T, bool>>? filter = null);

    #endregion
}