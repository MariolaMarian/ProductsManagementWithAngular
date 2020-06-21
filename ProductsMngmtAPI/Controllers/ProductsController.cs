using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsMngmt.DAL.Repositories;
using ProductsMngmt.BLL.Models;
using AutoMapper;
using ProductsMngmt.ViewModels.VMs.Product;
using System.Collections.Generic;
using System;
using ProductsMngmt.ViewModels.DTOs.Product;
using System.Linq.Expressions;
using ProductsMngmtAPI.Helpers.QueryParams;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using ProductsMngmtAPI.Helpers.Extensions;
using ProductsMngmt.DAL.Helpers.Pagination;
using ProductsMngmtAPI.Helpers.Headers;
using ProductsMngmt.DAL.UnitsOfWork;

namespace ProductsMngmtAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductCategoryUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public ProductsController(IProductCategoryUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET api/products
        /// <summary>
        /// Returns paginated products
        /// </summary>
        /// <remarks>If there is no order selected products will be sorted by their names ascending</remarks>
        /// <response code="200">Products found</response>
        /// <response code="404">No product was found</response>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductsFilterParams filterParams)
        {
            string userIdToFilter = (User.IsInRole("Admin") || User.IsInRole("Manager")) ? null : (await _userManager.GetUserAsync(User)).Id;

            Expression<Func<Product, bool>> filters = filterParams.ProductsFiltersAsOneExpression(userIdToFilter);

            OrderHeader orderHeader = new OrderHeader(
                    filterParams.ByProductName, filterParams.ByProductNameDesc,
                    filterParams.ByCategoryName, filterParams.ByCategoryNameDesc,
                    filterParams.ByExpDate, filterParams.ByExpDateDesc);

            PagedList<Product> paginatedProducts = await _unitOfWork.ProductRepository.GetPaginated(filters,
                orderHeader.ReturnOrderedQueryable(),
                filterParams, x => x.ExpirationDates);

            Response.AddPagination(paginatedProducts.CurrentPage, paginatedProducts.PageSize, paginatedProducts.TotalCount, paginatedProducts.TotalPages);

            Response.AddProductsFilters(filterParams.Barecode, filterParams.ProductName, filterParams.CategoriesIds);

            Response.AddOrder(filterParams.ByProductName, filterParams.ByProductNameDesc, filterParams.ByCategoryName, filterParams.ByCategoryNameDesc, filterParams.ByExpDate, filterParams.ByExpDateDesc);

            var productsToReturn = _mapper.Map<IEnumerable<ProductVM>>(paginatedProducts.Items);

            if (productsToReturn == null)
                return NotFound();

            return Ok(productsToReturn);
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            string userIdToFilter = (User.IsInRole("Admin") || User.IsInRole("Manager")) ? null : (await _userManager.GetUserAsync(User)).Id;

            var product = await _unitOfWork.ProductRepository.Get(id.ProductFiltersAsOneExpression(userIdToFilter), null, x => x.Category);

            var productToReturn = _mapper.Map<ProductVM>(product);

            if (productToReturn == null)
                return NotFound();
            else
                return Ok(productToReturn);
        }

        [Authorize(Policy = "RequireMediumRole")]
        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCreateDTO product)
        {
            if ((await _unitOfWork.ProductRepository.Get(x => x.Barecode == product.Barecode)) != null)
            {
                return BadRequest("Product with this barecode already exists!");
            }

            if (!(await IsAllowedToPostOrPutProduct(product.CategoryId)))
                return Unauthorized("You are not allowed to add product to this category");

            var productEntity = _mapper.Map<Product>(product);
            _unitOfWork.ProductRepository.Add(productEntity);

            try
            {
                if (await _unitOfWork.SaveAll())
                {
                    return CreatedAtRoute("GetProduct", new { id = productEntity.Id }, _mapper.Map<ProductVM>(productEntity));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
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

            if (!(await IsAllowedToPostOrPutProduct(product.CategoryId)))
                return Unauthorized("You are not allowed to edit product in this category");

            var existingProduct = await _unitOfWork.ProductRepository.Get(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var productEntity = _mapper.Map<Product>(product);

            existingProduct.Name = productEntity.Name;
            existingProduct.CategoryId = productEntity.CategoryId;
            existingProduct.MaxDays = productEntity.MaxDays;
            existingProduct.Image = productEntity.Image;

            _unitOfWork.ProductRepository.Update(existingProduct);

            if (await _unitOfWork.SaveAll())
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
            var productToDelete = (await _unitOfWork.ProductRepository.Get(x => x.Id == id));

            if (productToDelete == null)
            {
                return BadRequest("This product doesn't exist!");
            }

            _unitOfWork.ProductRepository.Delete(productToDelete);

            try
            {
                if (await _unitOfWork.SaveAll())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return BadRequest("Error while trying to delete product");
        }

        private async Task<bool> IsAllowedToPostOrPutProduct(int categoryId)
        {
            bool allowed = (User.IsInRole("Admin") || User.IsInRole("Manager"));
            //if user is not admin or manager check if he is assigned to this category
            if (!allowed)
            {
                string userIdToFilter = (await _userManager.GetUserAsync(User)).Id;
                allowed = await _unitOfWork.CategoryRepository.Get(c => c.CategoryUsers.Any(x => x.CategoryId == categoryId && x.UserId == userIdToFilter)) != null;
            }
            return allowed;
        }
    }
}