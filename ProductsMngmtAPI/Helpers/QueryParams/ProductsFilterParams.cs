using System.Collections.Generic;
using ProductsMngmtAPI.Helpers.Pagination;

namespace ProductsMngmtAPI.Helpers.QueryParams
{
    public class ProductsFilterParams : PaginationParams
    {
        public List<int> CategoriesIds { get; set; }
        public string Barecode { get; set; }
        public string ProductName { get; set; }
        public bool ByProductName {get;set;}
        public bool ByProductNameDesc {get;set;}
        public bool ByCategoryName {get;set;}
        public bool ByCategoryNameDesc {get;set;}
        public bool ByExpDate {get;set;}
        public bool ByExpDateDesc {get;set;}
    }
}