using System;
using System.Threading.Tasks;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.DAL.Repositories;

namespace ProductsMngmt.DAL.UnitsOfWork
{
    public interface IProductCategoryUnitOfWork : IDisposable
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        Task<bool> SaveAll();
    }
}