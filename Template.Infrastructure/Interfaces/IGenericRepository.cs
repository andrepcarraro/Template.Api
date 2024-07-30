using Template.Domain;

namespace Template.Infrastructure;
public interface IGenericRepository<T, TId> where T : IEntity<TId>
{
    Task<Result<T>> GetByIdAsync(TId id);
    Task<Result<IEnumerable<T>>> GetAllAsync();
    Task<Result<T>> AddAsync(T entity);
    Task<Result<IList<T>>> AddAllAsync(IList<T> entities);
    Result<T> Update(T entity);
    Task<Result<T>> DeleteAsync(TId id);
    IQueryable<T> AsQueryable();
}
