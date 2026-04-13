using Microsoft.EntityFrameworkCore;
namespace Planora.DataAccess.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    protected Repository(DbContext dbContext)
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
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(Guid.Parse(id)) ?? throw new KeyNotFoundException($"Key {id} does not exist.");
    }
    
    public async System.Threading.Tasks.Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}