using AutoMapper;
using Template.Api.Properties;
using Template.Application;
using Template.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers.v1;

public abstract class GenericController<TEntity, TViewModel, TRequestModel, TId>(IGenericService<TEntity, TId> service, IMapper mapper) : ControllerBase where TEntity : class, IEntity<TId>
{
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(TId id)
    {
        var result = await service.GetByIdAsync(id);
        var succesAction = (Result<TEntity> result) => Ok(Result<TViewModel>.Success(mapper.Map<TViewModel>(result.Data)));

        return HandleResult(result, succesAction);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll([FromQuery] GetAllRequestParams request)
    {
        var result = await service.GetAllAsync(request);
        var succesAction = (Result<IEnumerable<TEntity>> result) =>
        {
            var viewModelItens = mapper.Map<IEnumerable<TViewModel>>(result.Data);
            var viewModelResult = Result<IEnumerable<TViewModel>>.Success(viewModelItens);
            viewModelResult.Pagination = result.Pagination;

            return Ok(viewModelResult);
        };
         
        return HandleResult(result, succesAction);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Add([FromBody] TRequestModel requestModel)
    {
        var entity = mapper.Map<TEntity>(requestModel);
        var result = await service.AddAsync(entity);

        var succesAction = (Result<TEntity> result) =>
        {
            var viewModel = mapper.Map<TViewModel>(result.Data);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, viewModel);
        };

        return HandleResult(result, succesAction);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update([FromBody] TRequestModel requestModel)
    {
        var entity = mapper.Map<TEntity>(requestModel);
        var result = await service.UpdateAsync(entity);

        var succesAction = (Result<TEntity> result) => Ok(Result<TViewModel>.Success(mapper.Map<TViewModel>(result.Data)));

        return HandleResult(result, succesAction);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(TId id)
    {
        var result = await service.DeleteAsync(id);

        var succesAction = (Result<TEntity> result) => NoContent();

        return HandleResult(result, succesAction);
    }

    public ActionResult HandleResult<T>(Result<T> result, Func<Result<T>, ActionResult> succesAction)
    {
        if (result.IsSuccess)
            return succesAction(result);

        if (result.Error == null)
            throw new Exception(Resources.GenericControllerErrorDetailsValidation);

        return CreateErrorResponse(result.Error.StatusCode, result.Error.Message);
    }

    private ObjectResult CreateErrorResponse(int statusCode, string message)
    {
        var errorResponse = new
        {
            StatusCode = statusCode,
            ErrorMessage = message
        };

        return StatusCode(statusCode, errorResponse);
    }
}

