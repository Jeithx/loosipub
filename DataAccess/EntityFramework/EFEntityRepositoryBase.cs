using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework;

public class EFEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, new()
    where TContext : DbContext, new()
{
    public TEntity Add(TEntity entity)
    {
        using (var _context = new TContext())
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();
            return entity;
        }
    }

    public bool Delete(TEntity entity)
    {
        using (var _context = new TContext())
        {
            try
            {
                var deletedEntity = _context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public TEntity Get(Expression<Func<TEntity, bool>>? filter, string[]? includes = null)
    {
        using (var _context = new TContext())
        {
            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]).AsQueryable();

            return model.FirstOrDefault(filter);
        }
    }

    public bool Any(Expression<Func<TEntity, bool>> filter)
    {
        using (var _context = new TContext())
        {
            return _context.Set<TEntity>().Any(filter);
        }
    }

    public IList<TEntity> GetList(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null)
    {
        using (var _context = new TContext())
        {
            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]).AsQueryable();

            return model.ToList();
        }
    }

    public bool Update(TEntity entity)
    {
        using (var _context = new TContext())
        {
            try
            {
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public bool SoftDelete(TEntity entity)
    {
        using (var _context = new TContext())
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }
    }

    public TEntity Find(int id)
    {
        using (var _context = new TContext())
        {
            var model = _context.Set<TEntity>().Find();
            if (model != null)
                return model;
            return new TEntity();
        }
    }

    public IEnumerable<TEntity> GetPage(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null,
        int pageNumber = 1, int pageSize = 1)
    {
        using (var _context = new TContext())
        {
            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]).AsQueryable();

            return model.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }

    public IEnumerable<TEntity> GetFromEnd(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null,
        Expression<Func<TEntity, long>>? orderBy = null, int count = 10)
    {
        using (var _context = new TContext())
        {
            if (count <= 0)
                throw new ArgumentException("Çekilecek data 0'dan küçük olamaz.");

            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]).AsQueryable();

            return model.OrderByDescending(orderBy).Take(count).ToList();
        }
    }

    public IEnumerable<TEntity> GetFromStart(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null,
        Expression<Func<TEntity, long>>? orderBy = null, int count = 10)
    {
        using (var _context = new TContext())
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero.");

            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]).AsQueryable();

            return model.OrderBy(orderBy).Take(count).ToList();
        }
    }

    public int GetTotalCount(Expression<Func<TEntity, bool>>? filter = null)
    {
        using (var _context = new TContext())
        {
            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            return model.Count();
        }
    }

    public IEnumerable<TEntity> Search(Func<TEntity, bool> predicate, Expression<Func<TEntity, bool>>? filter = null,
        string[]? includes = null, int pageNumber = 1, int pageSize = 1)
    {
        using (var _context = new TContext())
        {
            var model = filter != null
                ? _context.Set<TEntity>().Where(filter).AsQueryable()
                : _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]).AsQueryable();

            return model.Where(predicate).AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }

    public int Count(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null)
    {
        using (var _context = new TContext())
        {
            var model = _context.Set<TEntity>().AsQueryable();
            if (includes != null)
                for (var i = 0; i < includes.Length; i++)
                    model = model.Include(includes[i]);

            return filter != null ? model.Count(filter) : model.Count();
        }
    }
}