using Template.Application.Properties;
using Template.Domain;
using Template.Domain.Common;
using Template.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Template.Application;

internal abstract class GenericService<T, TId> : IGenericService<T, TId> where T : class, IEntity<TId>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<T, TId> _repository;

    public GenericService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = GetRepository();
    }

    public virtual async Task<Result<T>> GetByIdAsync(TId id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<Result<IEnumerable<T>>> GetAllAsync(GetAllRequestParams request)
    {
        var query = _repository.AsQueryable();

        foreach (var filter in request.Filters)
        {
            var propertyName = filter.Key;
            var propertyValue = filter.Value;

            if (!string.IsNullOrEmpty(propertyValue))
            {
                var propertyInfo = typeof(T).GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    var propertyType = propertyInfo.PropertyType;

                    // Convert the propertyValue to the appropriate type
                    var convertedValue = Convert.ChangeType(propertyValue, propertyType);

                    // Build the expression tree dynamically
                    var parameter = Expression.Parameter(typeof(T), "e");
                    var propertyExpression = Expression.Property(parameter, propertyName);
                    var constantExpression = Expression.Constant(convertedValue);
                    var containsMethod = propertyType.GetMethod("Contains", new[] { propertyType });

                    Expression filterExpression;
                    if (containsMethod != null && propertyType == typeof(string))
                        filterExpression = Expression.Call(propertyExpression, containsMethod, constantExpression);
                    else
                        filterExpression = Expression.Equal(propertyExpression, constantExpression);

                    var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                    query = query.Where(lambda);
                }
            }
        }

        var paginationParams = request.Pagination;

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        return Result<IEnumerable<T>>.Success(items, paginationParams.PageSize, paginationParams.PageNumber, totalCount);
    }

    public virtual async Task<Result<IEnumerable<T>>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task<Result<T>> AddAsync(T entity)
    {
        var result = await _repository.AddAsync(entity);
        return HandleAffectedLines(await _unitOfWork.SaveChangesAsync(), result, Resources.NoLinesAffectedValidation);
    }

    public virtual async Task<Result<IList<T>>> AddAllAsync(IList<T> entities)
    {
        var result = await _repository.AddAllAsync(entities);
        return HandleAffectedLines(await _unitOfWork.SaveChangesAsync(), result, Resources.NoLinesAffectedValidation);
    }

    public virtual async Task<Result<T>> UpdateAsync(T entity)
    {
        var result = _repository.Update(entity);
        return HandleAffectedLines(await _unitOfWork.SaveChangesAsync(), result, Resources.NoLinesAffectedValidation);
    }

    public virtual async Task<Result<T>> DeleteAsync(TId id)
    {
        var result = await _repository.DeleteAsync(id);
        return HandleAffectedLines(await _unitOfWork.SaveChangesAsync(), result, Resources.NoLinesAffectedValidation);
    }

    private IGenericRepository<T, TId> GetRepository()
    {
        // Usando reflexão para acessar a propriedade do UnitOfWork que corresponde ao tipo T
        var propertyName = typeof(T).Name + "Repository";
        var propertyInfo = _unitOfWork.GetType().GetProperty(propertyName) ?? throw new InvalidOperationException($"Property '{propertyName}' not found on unit of work.");
        var repository =  (IGenericRepository<T, TId>?)propertyInfo.GetValue(_unitOfWork);

        return repository ?? throw new InvalidOperationException($"Property '{propertyName}' not found on unit of work.");
    }

    public Result<TReturn> HandleAffectedLines<TReturn>(int affectedLines, Result<TReturn> result, string message)
    {
        if (affectedLines > 0)
            return result;

        return HandleNotAffectedLines<TReturn>(message);
    }

    public Result<TReturn> HandleNotAffectedLines<TReturn>(string message)
    {
        return Result<TReturn>.Failure(HttpStatusCode.NotModified, message);
    }
}

