using ProductsMngmt.DAL.Helpers.Pagination;
using System.Collections.Generic;

namespace ProductsMngmtAPI.Helpers.QueryParams
{
    public class ProductsFilterParams : PaginationParams
    {
        /// <summary>
        /// Part of barecode which products have to have
        /// </summary>
        /// <example>165</example>
        public string Barecode { get; set; }
        /// <summary>
        /// Part of product name which products have to have
        /// </summary>
        /// <example>sandw</example>
        public string ProductName { get; set; }
        /// <summary>
        /// Categories in which products should be looked for
        /// </summary>
        /// <example>1</example>
        public List<int> CategoriesIds { get; set; }
        /// <summary>
        /// If products should be ordered by products name, if false there will be no order by products name
        /// </summary>
        /// <example>true</example>
        public bool ByProductName { get; set; }
        /// <summary>
        /// If false - products names will be sorted ascending, if true - products names will be sorted descending. Remember to set byProductName to true if there should be any sorting applied to products names
        /// </summary>
        /// <example>true</example>
        public bool ByProductNameDesc { get; set; }
        /// <summary>
        /// If products should be ordered by category name, if false there will be no order by category name
        /// </summary>
        /// <example>true</example>
        public bool ByCategoryName { get; set; }
        /// <summary>
        /// If false - products will be sorted ascending by category name, if true - products will be sorted descending by category name. Remember to set byCategoryName to true if there should be any sorting applied to products category names
        /// </summary>
        /// <example>true</example>
        public bool ByCategoryNameDesc { get; set; }
        /// <summary>
        /// If products should be ordered by nearest expiration date, if false there will be no order by products nearest expiration date
        /// </summary>
        /// <example>true</example>
        public bool ByExpDate { get; set; }
        /// <summary>
        /// If false - products will be sorted ascending by their nearest expiration date, if true - products will be sorted descending by products nearest expiration dates. Remember to set ByExpDateDesc to true if there should be any sorting applied to products neares expiration dates
        /// </summary>
        /// <example>true</example>
        public bool ByExpDateDesc { get; set; }
    }
}