using System;
using System.Threading.Tasks;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.DAL.Data;
using ProductsMngmt.DAL.Repositories;

namespace ProductsMngmt.DAL.UnitsOfWork
{
    public class ProductCategoryUnitOfWork : IProductCategoryUnitOfWork
    {
        private DataContext _dataContext;
        private IRepository<Product> _productRepository;
        private IRepository<Category> _categoryRepository;

        public ProductCategoryUnitOfWork(DataContext dataContext, IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _dataContext = dataContext;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IRepository<Product> ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new Repository<Product>(_dataContext);
                }
                return _productRepository;
            }
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