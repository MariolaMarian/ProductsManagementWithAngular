using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsMngmtAPI.Data.IRepos;
using ProductsMngmtAPI.Models;
using ProductsMngmtAPI.Helpers.Pagination;
using AutoMapper;
using ProductsMngmtAPI.VMs.Product;
using System.Collections.Generic;
using System;
using ProductsMngmtAPI.DTOs.Product;
using System.Linq.Expressions;
using ProductsMngmtAPI.Helpers.QueryParams;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using ProductsMngmtAPI.Helpers.Extensions;

namespace ProductsMngmtAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _repo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public ProductsController(IRepository<Product> repo, IMapper mapper, UserManager<User> userManager)
        {
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductsFilterParams filterParams)
        {
            string userIdToFilter =  (User.IsInRole("Admin") || User.IsInRole("Manager")) ? null : (await _userManager.GetUserAsync(User)).Id;

            Expression<Func<Product, bool>> filters = filterParams.GetAsOneExpression(userIdToFilter);

            PagedList<Product> paginatedProducts = await _repo.GetPaginated(filters, dbSet => dbSet.OrderBy(p => p.Name), filterParams, x => x.ExpirationDates);

            Response.AddPagination(paginatedProducts.CurrentPage, paginatedProducts.PageSize, paginatedProducts.TotalCount, paginatedProducts.TotalPages);

            Response.AddProductsFilters(filterParams.Barecode, filterParams.ProductName, filterParams.CategoriesIds);

            Response.AddOrder(filterParams.ByProductName, filterParams.ByProductNameDesc, filterParams.ByCategoryName, filterParams.ByCategoryNameDesc, filterParams.ByExpDate, filterParams.ByExpDateDesc);

            var productsToReturn = _mapper.Map<IEnumerable<ProductVM>>(paginatedProducts.Items);
            return Ok(productsToReturn);
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repo.Get(x => x.Id == id, null, x => x.Category);

            var productToReturn = _mapper.Map<ProductVM>(product);
            return Ok(productToReturn);
        }

        [Authorize(Policy = "RequireMediumRole")]
        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCreateDTO product)
        {
            if ((await _repo.Get(x => x.Barecode == product.Barecode)) != null)
            {
                return BadRequest("Product with this barecode already exists!");
            }

            var productEntity = _mapper.Map<Product>(product);
            _repo.Add(productEntity);

            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("GetProduct", new { id = productEntity.Id }, _mapper.Map<ProductVM>(productEntity));
            }

            return BadRequest("Error during adding new product");
        }

        [Authorize(Policy = "RequireMediumRole")]
        // PUT api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductEditDTO product)
        {
            if (id != product.Id)
            {
                return BadRequest("Ids don't match");
            }

            var existingProduct = await _repo.Get(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var productEntity = _mapper.Map<Product>(product);

            existingProduct.Name = productEntity.Name;
            existingProduct.CategoryId = productEntity.CategoryId;
            existingProduct.MaxDays = productEntity.MaxDays;
            existingProduct.Image = productEntity.Image;

            _repo.Update(existingProduct);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Error while editing product");
        }

        [Authorize(Policy = "RequireHighestRole")]
        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productToDelete = (await _repo.Get(x => x.Id == id));

            if (productToDelete == null)
            {
                return BadRequest("This product doesn't exist!");
            }

            _repo.Delete(productToDelete);

            if (await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error while trying to delete product");
        }
    }
}