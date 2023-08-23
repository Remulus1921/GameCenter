using GameCenter.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Core.Repositories.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<bool> Add(T entity)
    {
        _dbSet.Add(entity);
        return true;
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<bool> Delete(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Update(T entity)
    {
        _dbSet.Update(entity);
        return true;
    }
}
