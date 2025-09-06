using Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace CorexPack.DataAccess.EntityFramework;

public class EFEntityRepositoryBaseAsync<TEntity, TContext> : IEntityRepositoryAsync<TEntity>
    where TEntity : class, new()
    where TContext : DbContext, new() // Added new() constraint to TContext
{
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        using (var _context = new TContext())
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, string[]? includes = null)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            return await _context.Set<TEntity>().AnyAsync(filter);
        }
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null,
        string[]? includes = null)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            _context.Set<TEntity>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }

    public async Task<TEntity?> FindAsync(int id)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }

    public async Task<List<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>>? filter = null,
        string[]? includes = null, int pageNumber = 1, int pageSize = 10)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includes = null)
    {
        using (var _context = new TContext()) // Added using statement for _context
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }

    public async Task<List<TEntity>> GetPageAsync<TKey>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TKey>>? orderByDescending = null,
        Expression<Func<TEntity, TKey>>? orderBy = null,
        string[]? includes = null,
        int pageNumber = 1,
        int pageSize = 10,
        Expression<Func<TEntity, TKey>>? groupBy = null)
    {
        using (var _context = new TContext())
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            // Apply grouping if specified
            if (groupBy != null)
                // Group and order within groups
                query = query
                    .GroupBy(groupBy)
                    .SelectMany(group => group); // Flatten the groups back to a list

            if (orderBy != null)
                query = query.OrderBy(orderBy);
            else if (orderByDescending != null)
                query = query.OrderByDescending(orderByDescending);
            else
                query = query.OrderBy(e => EF.Property<object>(e, "Id"));

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}