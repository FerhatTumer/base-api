using System.Linq.Expressions;

namespace TaskManagement.Domain.Common;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
{
    Task AddAsync(T entity, CancellationToken cancellationToken);

    void Update(T entity);

    void Remove(T entity);

    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
}