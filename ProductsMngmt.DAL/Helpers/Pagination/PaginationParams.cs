namespace ProductsMngmt.DAL.Helpers.Pagination
{
    public class PaginationParams
    {
        private const int MAX_PAGE_SIZE = 50;
        private int pageSize = 12;
        /// <summary>
        /// Which page should be returned, by default 1 - the first one
        /// </summary>
        /// <example>165</example>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// How many items should be returned, by default 12, max - 50
        /// </summary>
        /// <example>165</example>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value; }
        }
    }
}