using Template.Domain.Common;
using System.Drawing.Printing;
using System.Net;

namespace Template.Domain;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public ErrorDetails? Error { get; set; }
    public T? Data { get; set; }

    public PaginationDetails? Pagination { get; set; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }

    public static Result<T> Success(T data, int pageSize, int pageNumber, int totalCount)
    {
        return new Result<T> { IsSuccess = true, Data = data, Pagination=new PaginationDetails(pageSize, pageNumber, totalCount) };
    }

    public static Result<T> Failure(HttpStatusCode statusCode, string errorMessage)
    {
        return new Result<T> { IsSuccess = false, Error = new ErrorDetails(statusCode, errorMessage) };
    }
}

public class ErrorDetails(HttpStatusCode StatusCode, string Message)
{
    public int StatusCode { get; private set; } = (int)StatusCode;
    public string Message { get; private set; } = Message;
}

public class PaginationDetails : PaginationParams
{
    public PaginationDetails(int pageSize, int pageNumber, int totalCount)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.TotalCount = totalCount;
    }
    public int TotalCount { get; set; } = 0;
}
