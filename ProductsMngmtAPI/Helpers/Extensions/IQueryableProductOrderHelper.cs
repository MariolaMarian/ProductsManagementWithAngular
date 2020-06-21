using System;
using System.Linq;
using ProductsMngmtAPI.Helpers.Headers;
using ProductsMngmt.BLL.Models;

namespace ProductsMngmtAPI.Helpers.Extensions
{
    public static class IQueryableProductOrderHelper
    {
        public static Func<IQueryable<Product>, IOrderedQueryable<Product>> ReturnOrderedQueryable(this OrderHeader orderHeader)
        {
            //if order is not set order by nearest product expiration date
            if (!(orderHeader.ByCategoryName || orderHeader.ByExpDate || orderHeader.ByProductName))
                return query => query.OrderBy(p => p.Name);

            Func<IQueryable<Product>, IOrderedQueryable<Product>> query = null;
            if (orderHeader.ByExpDate)
            {
                if (orderHeader.ByExpDateDesc)
                    query = query => query.OrderByDescending(p => p.ExpirationDates.Max(exp => exp.EndDate));
                else
                    query = query => query.OrderBy(p => p.Category.Name);
            }

            if (orderHeader.ByProductName)
            {
                if (orderHeader.ByProductNameDesc)
                    query = query => query.OrderByDescending(p => p.Name);
                else
                    query = query => query.OrderBy(p => p.Name);
            }

            if (orderHeader.ByCategoryName)
            {
                if (orderHeader.ByCategoryNameDesc)
                    query = query => query.OrderByDescending(p => p.Category.Name);
                else
                    query = query => query.OrderBy(p => p.Category.Name);
            }

            return query;
        }
    }
}