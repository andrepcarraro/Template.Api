using Template.Domain.Common;

namespace Template.Domain
{
    public class GetAllRequestParams
    {
        public PaginationParams Pagination { get; set; } = new PaginationParams();

        public Dictionary<string, string> Filters { get; set; } = [];
    }
}
