using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsMngmt.ViewModels.DTOs.Category;
using ProductsMngmtAPI.Helpers.Extensions;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.ViewModels.VMs.Category;
using ProductsMngmt.DAL.Helpers.Pagination;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using System;
using ProductsMngmt.DAL.UnitsOfWork;

namespace ProductsMngmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryUserCategoryUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public CategoriesController(ICategoryUserCategoryUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        ///<summary>
        ///Gets category view model
        ///</summary>
        ///<param name="id"></param>
        ///<response code="200">Returns category view model</response>
        ///<response code="404">Returns if id was in positive range but category was not found</response>
        ///<response code="400">Returns if id was below 1</response>
        [Authorize]
        [HttpGet("{id}", Name = "GetCategory")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategorySimpleVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (id < 1)
                return BadRequest();

            string userIdToFilter = (User.IsInRole("Admin") || User.IsInRole("Manager")) ? null : (await _userManager.GetUserAsync(User)).Id;

            var category = await _unitOfWork.CategoryRepository.Get(x => x.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        ///<summary>
        ///Gets paginated categories view model
        ///</summary>
        ///<param name="paginationParams"></param>
        ///<response code="200">Returns categories list in body and pagination parameters in header. If user is not admin or manager, only categories to which user is assigned are returned</response>
        [Authorize]
        [Route("categories")]
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] PaginationParams paginationParams)
        {
            PagedList<Category> pagedCategories = await _unitOfWork.CategoryRepository.GetPaginated((await FilterByUserId()), null, paginationParams, x => x.Products);

            Response.AddPagination(pagedCategories.CurrentPage, pagedCategories.PageSize, pagedCategories.TotalCount, pagedCategories.TotalPages);

            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryWithCountsVM>>(pagedCategories.Items);
            return Ok(categoriesToReturn);
        }

        // GET api/categoriesSimple
        ///<summary>
        ///Gets simplest informations about categories - not paginated (for select lists)
        ///</summary>
        ///<response code="200">Returns simplest informations about categories. If user is not admin or manager, only categories to which user is assigned are returned </response>
        [Authorize]
        [Route("categoriesSimple")]
        [HttpGet]
        public async Task<IActionResult> GetCategoriesSimple()
        {
            var categories = (await _unitOfWork.CategoryRepository.GetAll((await FilterByUserId()), null, null, null));

            var categoriesToReturn = _mapper.Map<IEnumerable<CategorySimpleVM>>(categories);

            return Ok(categoriesToReturn);
        }

        // POST api/categories
        ///<summary>
        /// Creates new category and assigns every manager to this category. Only for admins and managers
        ///</summary>
        ///<response code="201">Returns created category</response>
        ///<response code="400">If category already exists</response>
        ///<response code="500">If error with saving category or assigning manager to category occured in db</response>
        [Authorize(Policy = "RequireHighestRole")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDTO category)
        {
            if ((await _unitOfWork.CategoryRepository.Get(x => x.Name == category.Name)) != null)
            {
                return BadRequest("Category with this name already exists!");
            }

            var categoryEntity = _mapper.Map<Category>(category);
            _unitOfWork.CategoryRepository.Add(categoryEntity);

            try
            {
                if (await _unitOfWork.SaveAll())
                {
                    if(await AddManagersToCategory(categoryEntity.Id))
                        return CreatedAtRoute("GetCategory", new { id = categoryEntity.Id }, _mapper.Map<CategorySimpleVM>(categoryEntity));
                    else
                        return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


            return BadRequest("Could not add category");
        }

        /*
        // PUT api/categories/5
        [Authorize(Policy = "RequireHighestRole")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CategoryUpdateDTO category)
        {
        }
        */

        // DELETE api/categories/5

        ///<summary>
        /// Deletes category with all products and their expiration dates. Only for admins and managers
        ///</summary>
        ///<response code="200">Returns only status code if deleting succeeded</response>
        ///<response code="400">If category does not exist</response>
        ///<response code="500">If error with deleting from db occured</response>
        [Authorize(Policy = "RequireHighestRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryToDelete = (await _unitOfWork.CategoryRepository.Get(x => x.Id == id));

            if (categoryToDelete == null)
            {
                return BadRequest("This category doesn't exist!");
            }

            _unitOfWork.CategoryRepository.Delete(categoryToDelete);
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

            return BadRequest("Error while trying to delete category");
        }

        private async Task<Expression<Func<Category, bool>>> FilterByUserId()
        {
            string userIdToFilter = (User.IsInRole("Admin") || User.IsInRole("Manager")) ? null : (await _userManager.GetUserAsync(User)).Id;
            Expression<Func<Category, bool>> exprBase = PredicateBuilder.New<Category>(true);

            if (userIdToFilter != null)
            {
                exprBase = exprBase.And(c => c.CategoryUsers.Any(u => u.UserId == userIdToFilter));
            }

            return exprBase;
        }

        private async Task<bool> AddManagersToCategory(int categoryId)
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            foreach (var manager in managers)
            {
                _unitOfWork.UserCategoryRepository.Add(new UserCategory() { UserId = manager.Id, CategoryId = categoryId });
            }
            try
            {
                await _unitOfWork.SaveAll();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
