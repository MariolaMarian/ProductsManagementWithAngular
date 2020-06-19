using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsMngmtAPI.Data.IRepos;
using ProductsMngmtAPI.Models;
using AutoMapper;
using ProductsMngmtAPI.Helpers.Pagination;
using ProductsMngmtAPI.Helpers;
using System.Collections.Generic;
using ProductsMngmtAPI.DTOs.ExpirationDate;
using System.Security.Claims;
using System.Linq.Expressions;
using System;
using ProductsMngmtAPI.Helpers.QueryParams;
using ProductsMngmtAPI.VMs.ExpirationDate;
using LinqKit;
using System.Linq;
using ProductsMngmtAPI.Helpers.Extensions;

namespace ProductsMngmtAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpirationDatesController : Controller
    {
        private readonly IRepository<ExpirationDate> _repo;
        private readonly IMapper _mapper;
        public ExpirationDatesController(IRepository<ExpirationDate> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET api/expirationDates
        [HttpGet]
        public async Task<IActionResult> GetExpirationDates([FromQuery] ExpirationDatesFilterParams filterParams)
        {
            Expression<Func<ExpirationDate, object>>[] includings = new Expression<Func<ExpirationDate, object>>[2];

            includings[0] = x => x.Product;

            if (filterParams.WithEmployees)
            {
                includings[1] = x => x.CollectedBy;
            }

            Expression<Func<ExpirationDate, bool>> filtersBase = PredicateBuilder.New<ExpirationDate>(true);

            if (filterParams.ProductId > 0)
            {
                filtersBase = filtersBase.And(x => x.ProductId == filterParams.ProductId);
            }

            PagedList<ExpirationDate> paginatedExpirationDates = await _repo.GetPaginated(filtersBase, dbSet => dbSet.OrderBy(exp => exp.Collected ? 1: 0).ThenBy(exp => exp.EndDate), filterParams, includings);

            Response.AddPagination(paginatedExpirationDates.CurrentPage, paginatedExpirationDates.PageSize, paginatedExpirationDates.TotalCount, paginatedExpirationDates.TotalPages);

            return Ok(_mapper.Map<IEnumerable<ExpirationDateVM>>(paginatedExpirationDates.Items));
        }

        // GET api/expirationDates/5
        [HttpGet("{id}", Name = "GetExpirationDate")]
        public async Task<IActionResult> GetExpirationDate(int id)
        {
            var expirationDate = await _repo.Get(x => x.Id == id, null, x => x.Product, x => x.CollectedBy);

            if (expirationDate == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ExpirationDateVM>(expirationDate));
        }

        // POST api/expirationDates
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpirationDateCreateDTO expirationDate)
        {

            if (await _repo.Get(x => x.EndDate.Date == expirationDate.EndDate.Date && x.ProductId == expirationDate.ProductId) != null)
            {
                return BadRequest("This expiration date already exists for this product!");
            }

            var expirationDateEntity = _mapper.Map<ExpirationDate>(expirationDate);
            _repo.Add(expirationDateEntity);

            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("GetExpirationDate", new { id = expirationDateEntity.Id }, _mapper.Map<ExpirationDateForCreateVM>(expirationDateEntity));
            }

            return BadRequest("Error during adding new expiration date");
        }

        // PUT api/expirationDates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ExpirationDateUpdateDTO expirationDate)
        {
            if (id != expirationDate.Id)
            {
                return BadRequest();
            }

            if (expirationDate.Count < 0)
            {
                return BadRequest("Count must be positiive number!");
            }

            var expirationDateEntity = await _repo.Get(x => x.Id == id);
            if (expirationDateEntity == null)
                return NotFound();

            if (expirationDateEntity.Count > expirationDate.Count)
            {
                return BadRequest("Cannot save expiration date with count less than was collected previously");
            }

            expirationDateEntity.Collected = true;
            expirationDateEntity.Count = expirationDate.Count;
            expirationDateEntity.CollectedDate = System.DateTime.Now;
            expirationDateEntity.CollectedById = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _repo.Update(expirationDateEntity);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Error during adding new expiration date");
        }

        // DELETE api/expirationDates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expirationDateToDelete = (await _repo.Get(x => x.Id == id));

            if (expirationDateToDelete == null)
            {
                return BadRequest("This expiration date doesn't exist!");
            }

            _repo.Delete(expirationDateToDelete);

            if (await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error while trying to delete expiration date");
        }
    }
}