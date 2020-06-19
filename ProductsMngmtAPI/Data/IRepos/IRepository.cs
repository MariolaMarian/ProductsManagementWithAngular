using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProductsMngmtAPI.Helpers.Pagination;

namespace ProductsMngmtAPI.Data.IRepos
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> order = null,  params Expression<Func<T, object>>[] includings);
        Task<PagedList<T>> GetPaginated(Expression<Func<T, bool>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, 
         PaginationParams paginationParams = null, params Expression<Func<T, object>>[] includings);

        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, 
        params Expression<Func<T, object>>[] includings);
        void Add(T entity);

        void Delete(T entity);

        Task Delete(Expression<Func<T, bool>> filters);

        void Update(T entity);

        Task<bool> SaveAll();
    }
}