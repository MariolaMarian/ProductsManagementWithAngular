using System;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductsMngmtAPI.Helpers.QueryParams;
using ProductsMngmt.BLL.Models;

namespace ProductsMngmtAPI.Helpers.Extensions
{
    public static class ProductsFiltersExtension
    {
        public static Expression<Func<Product, bool>> ProductsFiltersAsOneExpression(this ProductsFilterParams filterParams, string userIdToFilter)
        {
            Expression<Func<Product, bool>> exprBase =  PredicateBuilder.New<Product>(true);
            
            if (!String.IsNullOrEmpty(filterParams.Barecode))
            {
                exprBase = exprBase.And(p => p.Barecode.Contains(filterParams.Barecode));
            }

            if (!String.IsNullOrEmpty(filterParams.ProductName))
            {
                exprBase = exprBase.And(p => EF.Functions.Like(p.Name, "%" + filterParams.ProductName + "%"));
            }

            if (filterParams.CategoriesIds != null)
            {
                Expression<Func<Product, bool>> innerBase = PredicateBuilder.New<Product>(false);
                foreach (var categoryId in filterParams.CategoriesIds)
                {
                     innerBase = innerBase.Or(p => p.CategoryId == categoryId);
                }
                exprBase = exprBase.And(innerBase);
            }

            if(userIdToFilter != null)
            {
                exprBase = exprBase.And(p => p.Category.CategoryUsers.Any(u => u.UserId == userIdToFilter));
            }

            return exprBase;
        }

        public static Expression<Func<Product, bool>> ProductFiltersAsOneExpression(this int productId, string userIdToFilter)
        {
            Expression<Func<Product, bool>> exprBase = PredicateBuilder.New<Product>(true);

            exprBase = exprBase.And(p => p.Id == productId);

            if (userIdToFilter != null)
            {
                exprBase = exprBase.And(p => p.Category.CategoryUsers.Any(u => u.UserId == userIdToFilter));
            }

            return exprBase;
        }
    }
}