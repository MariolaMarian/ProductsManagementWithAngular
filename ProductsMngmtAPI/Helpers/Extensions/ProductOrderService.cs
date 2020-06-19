using System;
using System.Linq;
using ProductsMngmtAPI.Helpers.Headers;
using ProductsMngmtAPI.Models;

namespace ProductsMngmtAPI.Helpers.Extensions
{
    public static class IQueryableProductOrderHelper
    {
        public static IOrderedQueryable<Product> ReturnOrderedQueryable(this IQueryable<Product> query, OrderHeader orderHeader)
        {
            //if order is not set order by nearest product expiration date
            if (!(orderHeader.ByCategoryName || orderHeader.ByExpDate || orderHeader.ByProductName))
                return query.OrderBy(p => p.ExpirationDates.Max(ex => ex.EndDate));
            
            if (orderHeader.ByExpDate)
            {
                if (orderHeader.ByExpDateDesc)
                    query = query.OrderByDescending(p => p.ExpirationDates.Max(exp => exp.EndDate));
                else
                    query = query.OrderBy(p => p.Category.Name);
            }

            if (orderHeader.ByProductName)
            {
                if (orderHeader.ByProductNameDesc)
                    query = query.OrderByDescending(p => p.Name);
                else
                    query = query.OrderBy(p => p.Name);
            }

            if (orderHeader.ByCategoryName)
            {
                if (orderHeader.ByCategoryNameDesc)
                    query = query.OrderByDescending(p => p.Category.Name);
                else
                    query = query.OrderBy(p => p.Category.Name);
            }

            return (IOrderedQueryable<Product>)query;
        }
    }
}