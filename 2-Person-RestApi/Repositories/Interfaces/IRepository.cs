namespace Person_RestApi.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> AddAsync(T item);
    Task<T?> GetByIdAsync(int id);
    Task<T?> UpdateAsync(T item);
    Task<T?> DeleteAsync(T item);
    Task<T?> DeleteByIdAsync(int id);
}