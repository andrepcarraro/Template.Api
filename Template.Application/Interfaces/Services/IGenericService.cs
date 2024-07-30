using Template.Domain;

namespace Template.Application;
public interface IGenericService<T, TId> where T : IEntity<TId>
{
    Task<Result<T>> GetByIdAsync(TId id);
    Task<Result<IEnumerable<T>>> GetAllAsync(GetAllRequestParams request);
    Task<Result<IEnumerable<T>>> GetAllAsync();
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result<T>> DeleteAsync(TId id);
    Result<TReturn> HandleNotAffectedLines<TReturn>(string message);
    Result<TReturn> HandleAffectedLines<TReturn>(int affectedLines, Result<TReturn> result, string message);
}

