namespace Template.Domain.Common
{
    public class PaginationParams
    {
        public PaginationParams() { }
        public PaginationParams(int pageSize, int pageNumber)
        {
            PageSize = pageSize; 
            PageNumber = pageNumber;
        }

        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 1000;
    }
}
