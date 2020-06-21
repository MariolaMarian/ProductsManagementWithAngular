using System;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductsMngmt.BLL.Models;
using ProductsMngmtAPI.Helpers.QueryParams;

namespace ProductsMngmtAPI.Helpers.Extensions
{
    public static class ExpirationDatesFiltersExtension
    {
        public static Expression<Func<ExpirationDate, bool>> ExpirationDatesFiltersAsOneExpression(this ExpirationDatesFilterParams filterParams, string userIdToFilter)
        {
            Expression<Func<ExpirationDate, bool>> exprBase =  PredicateBuilder.New<ExpirationDate>(true);
            
            if (!String.IsNullOrEmpty(filterParams.Barecode))
            {
                exprBase = exprBase.And(exp => exp.Product.Barecode.Contains(filterParams.Barecode));
            }

            if (!String.IsNullOrEmpty(filterParams.ProductName))
            {
                exprBase = exprBase.And(exp => EF.Functions.Like(exp.Product.Name, "%" + filterParams.ProductName + "%"));
            }

            if (filterParams.CategoriesIds != null)
            {
                Expression<Func<ExpirationDate, bool>> innerBase = PredicateBuilder.New<ExpirationDate>(false);
                foreach (var categoryId in filterParams.CategoriesIds)
                {
                     innerBase = innerBase.Or(exp => exp.Product.CategoryId == categoryId);
                }
                exprBase = exprBase.And(innerBase);
            }

            if(!filterParams.IncludeCollected)
            {
                exprBase = exprBase.And(exp => !exp.Collected);
            }

            if(filterParams.ProductId > 0)
            {
                exprBase = exprBase.And(exp => exp.ProductId == filterParams.ProductId);
            }

            if(userIdToFilter != null)
            {
                exprBase = exprBase.And(exp => exp.Product.Category.CategoryUsers.Any(u => u.UserId == userIdToFilter));
            }

            return exprBase;
        }
    }
}