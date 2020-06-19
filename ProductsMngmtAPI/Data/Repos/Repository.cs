using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductsMngmtAPI.Data.IRepos;
using ProductsMngmtAPI.Helpers.Pagination;

namespace ProductsMngmtAPI.Data.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        DataContext _dataContext;
        DbSet<T> _dbSet;
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
            this._dbSet = dataContext.Set<T>();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filters, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params Expression<Func<T, object>>[] includings)
        {
            IQueryable<T> query = GetQueryableItems(filters,order,includings);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<PagedList<T>> GetPaginated(Expression<Func<T, bool>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, PaginationParams paginationParams = null, params Expression<Func<T, object>>[] includings)
        {
            IQueryable<T> query = GetQueryableItems(filters,order,includings);

            var filteredCount = await query.CountAsync();

            if (paginationParams != null)
            {
                query = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize);
            }
            else
            {
                paginationParams = new PaginationParams() { PageNumber = -1, PageSize = -1 };
            }

            return PagedList<T>.Create(await query.ToListAsync(), paginationParams.PageNumber, paginationParams.PageSize, filteredCount);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params Expression<Func<T, object>>[] includings)
        {
            IQueryable<T> query = GetQueryableItems(filters,order,includings);
            
            return await query.ToListAsync();
        }
        public void Add(T entity)
        {
            _dataContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _dataContext.Remove(entity);
        }

        public async Task Delete(Expression<Func<T, bool>> filters)
        {
            if (filters == null)
                return;

            IQueryable<T> query = this._dbSet;
            query = query.AsExpandable().Where(filters);

            var itemsToRemove = await query.ToListAsync();

            if (itemsToRemove != null && itemsToRemove.Count > 0)
                foreach (var itemToRemove in itemsToRemove)
                {
                    this.Delete(itemToRemove);
                }
        }

        public void Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> SaveAll()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        private IQueryable<T> GetQueryableItems(Expression<Func<T, bool>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params Expression<Func<T, object>>[] includings)
        {
            IQueryable<T> query = this._dbSet;

            if (includings != null)
            {
                includings.ToList().ForEach(including =>
                {
                    if (including != null)
                    {
                        query = query.Include(including);
                    }
                });
            }

            if (filters != null)
                query = query.AsExpandable().Where(filters);

            if (order != null)
                query = order(query);

            return query;
        }
    }
}
