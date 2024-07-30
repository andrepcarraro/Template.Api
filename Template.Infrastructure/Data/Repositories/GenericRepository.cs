using Template.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Template.Infrastructure;

internal abstract class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : class, IEntity<TId>
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<Result<T>> GetByIdAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity != null ? Result<T>.Success(entity) : Result<T>.Failure(HttpStatusCode.NotFound, "Entity not found");
    }

    public async Task<Result<IEnumerable<T>>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return entities.Count > 0 ? Result<IEnumerable<T>>.Success(entities) : Result<IEnumerable<T>>.Failure(HttpStatusCode.NotFound, "Entities not found");
    }

    public async Task<Result<T>> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return Result<T>.Success(entity);
    }

    public async Task<Result<IList<T>>> AddAllAsync(IList<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return Result<IList<T>>.Success(entities);
    }

    public Result<T> Update(T entity)
    {
        _dbSet.Update(entity);
        return Result<T>.Success(entity);
    }

    public async Task<Result<T>> DeleteAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) 
            return Result<T>.Failure(HttpStatusCode.NotFound, "Entity not found");

        _dbSet.Remove(entity);
        return Result<T>.Success(entity);
    }

    public IQueryable<T> AsQueryable()
    {
        return _dbSet.AsQueryable();
    }
}