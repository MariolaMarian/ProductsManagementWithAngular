using ProductsMngmtAPI.Helpers.Pagination;

namespace ProductsMngmtAPI.Helpers.QueryParams
{
    public class ExpirationDatesFilterParams : PaginationParams
    {
        public int ProductId { get; set; } = 0;
        public bool WithEmployees{get;set;} = false;
    }
}