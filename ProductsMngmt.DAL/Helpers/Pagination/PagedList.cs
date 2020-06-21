using System;
using System.Collections.Generic;

namespace ProductsMngmt.DAL.Helpers.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items {get;set;}

        public PagedList(int totalCount, int pageNumber, int pageSize, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items = items;
        }

        public static PagedList<T> Create(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
        {
            return new PagedList<T>(totalCount, pageNumber, pageSize, items);
        }
    }
}