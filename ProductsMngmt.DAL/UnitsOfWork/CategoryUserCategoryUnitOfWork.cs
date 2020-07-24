using System;
using System.Threading.Tasks;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.DAL.Data;
using ProductsMngmt.DAL.Repositories;

namespace ProductsMngmt.DAL.UnitsOfWork
{
    public class CategoryUserCategoryUnitOfWork : ICategoryUserCategoryUnitOfWork
    {
        private DataContext _dataContext;
        private IRepository<Category> _categoryRepository;
        private IRepository<UserCategory> _userCategoryRepository;

        public CategoryUserCategoryUnitOfWork(DataContext dataContext, IRepository<Category> categoryRepository, IRepository<UserCategory> userCategoryRepository)
        {
            _dataContext = dataContext;
            _categoryRepository = categoryRepository;
            _userCategoryRepository = userCategoryRepository;
        }

        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new Repository<Category>(_dataContext);
                }
                return _categoryRepository;
            }
        }

        public IRepository<UserCategory> UserCategoryRepository
        {
            get
            {
                if (_userCategoryRepository == null)
                {
                    _userCategoryRepository = new Repository<UserCategory>(_dataContext);
                }
                return _userCategoryRepository;
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}