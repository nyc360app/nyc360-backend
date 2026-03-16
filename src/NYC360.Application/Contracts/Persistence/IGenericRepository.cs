using System.Linq.Expressions;

namespace NYC360.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
    // Read Operations
    Task<T?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    
    // Write Operations
    Task AddAsync(T entity, CancellationToken ct);
    void Update(T entity);
    void Remove(T entity);
    
    // Utility
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}