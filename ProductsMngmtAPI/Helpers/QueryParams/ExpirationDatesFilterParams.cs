using ProductsMngmt.DAL.Helpers.Pagination;

namespace ProductsMngmtAPI.Helpers.QueryParams
{
    public class ExpirationDatesFilterParams : ProductsFilterParams
    {
        /// <summary>
        /// For which product expiration dates should be returned, by default = 0 which means from all products
        /// </summary>
        /// <example>2</example>
        public int ProductId { get; set; } = 0;
        /// <summary>
        /// If expiration dates which have been already collected should be displayed
        /// </summary>
        /// <example>2</example>
        public bool IncludeCollected { get; set; } = false;
        /// <summary>
        /// If information about who has collected expiration date should be included, by default false
        /// </summary>
        /// <example>2</example>
        public bool WithEmployees { get; set; } = false;
        /// <summary>
        /// If all information about expiration date product should be returned - for example image
        /// </summary>
        /// <example>2</example>
        public bool WithProducts { get; set; } = true;
    }
}