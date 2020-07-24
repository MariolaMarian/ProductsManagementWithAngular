using System;
using System.Threading.Tasks;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.DAL.Repositories;

namespace ProductsMngmt.DAL.UnitsOfWork
{
    public interface ICategoryUserCategoryUnitOfWork : IDisposable
    {
        IRepository<Category> CategoryRepository { get; }
        IRepository<UserCategory> UserCategoryRepository { get; }
        Task<bool> SaveAll();
    }
}