using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
namespace Planora.DataAccess.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly DatabaseContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    protected Repository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(Guid.Parse(id)) ?? throw new KeyNotFoundException($"Key {id} does not exist.");
    }
    
    public async void SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}