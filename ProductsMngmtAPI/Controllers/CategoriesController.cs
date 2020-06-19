using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsMngmtAPI.Data.IRepos;
using ProductsMngmtAPI.DTOs.Category;
using ProductsMngmtAPI.Helpers.Extensions;
using ProductsMngmtAPI.Helpers.Pagination;
using ProductsMngmtAPI.Models;
using ProductsMngmtAPI.VMs;
using ProductsMngmtAPI.VMs.Category;

namespace ProductsMngmtAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category> _repo;
        private readonly IMapper _mapper;
        public CategoriesController(IRepository<Category> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET api/categories/5
        ///<summary>
        ///Gets category view model
        ///</summary>
        ///<param name="id"></param>
        ///<response code="200">Returns category view model</response>
        ///<response code="404">Returns if id was in positive range but category was not found</response>
        ///<response code="400">Returns if id was below 1</response>
        [HttpGet("{id}", Name = "GetCategory")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategorySimpleVM),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategory(int id)
        {
            if(id < 1) 
                return BadRequest();

            var category = await _repo.Get(x => x.Id == id);

            if(category == null)
                return NotFound();

            return Ok(category);
        }

        // GET api/categories
        [Route("categories")]
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery]PaginationParams paginationParams)
        {
            PagedList<Category> pagedCategories = await _repo.GetPaginated(null, null, paginationParams, x => x.Products);

            Response.AddPagination(pagedCategories.CurrentPage, pagedCategories.PageSize, pagedCategories.TotalCount, pagedCategories.TotalPages);

            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryWithCountsVM>>(pagedCategories.Items);
            return Ok(categoriesToReturn);
        }

        // GET api/categoriesSimple
        [Route("categoriesSimple")]
        [HttpGet]
        public async Task<IActionResult> GetCategoriesSimple()
        {
            var categories = (await _repo.GetPaginated(null, null, null, null)).Items;
            
            var categoriesToReturn = _mapper.Map<IEnumerable<CategorySimpleVM>>(categories);
            
            return Ok(categoriesToReturn);
        }   

        // POST api/categories
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDTO category)
        {
            if ((await _repo.Get(x => x.Name == category.Name)) != null)
            {
                return BadRequest("Category with this name already exists!");
            }

            var categoryEntity = _mapper.Map<Category>(category);
            _repo.Add(categoryEntity);

            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("GetCategory", new { id = categoryEntity.Id }, _mapper.Map<CategorySimpleVM>(categoryEntity));
            }

            return BadRequest("Could not add category");
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]CategoryUpdateDTO category)
        {
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryToDelete = (await _repo.Get(x => x.Id == id));

            if (categoryToDelete == null)
            {
                return BadRequest("This category doesn't exist!");
            }

            _repo.Delete(categoryToDelete);

            if (await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error while trying to delete category");
        }
    }
}
