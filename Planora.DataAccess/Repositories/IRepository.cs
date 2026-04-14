namespace Planora.DataAccess.Repositories;

public interface IRepository<T> where T : class {
    Task<T> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    void SaveChangesAsync();
}