using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsMngmt.DAL.Repositories;
using ProductsMngmt.BLL.Models;
using AutoMapper;
using System.Collections.Generic;
using ProductsMngmt.ViewModels.DTOs.ExpirationDate;
using System.Security.Claims;
using System.Linq.Expressions;
using System;
using ProductsMngmtAPI.Helpers.QueryParams;
using ProductsMngmt.ViewModels.VMs.ExpirationDate;
using LinqKit;
using System.Linq;
using ProductsMngmtAPI.Helpers.Extensions;
using ProductsMngmt.DAL.Helpers.Pagination;
using Microsoft.AspNetCore.Identity;

namespace ProductsMngmtAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpirationDatesController : Controller
    {
        private readonly IRepository<ExpirationDate> _repo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public ExpirationDatesController(IRepository<ExpirationDate> repo, UserManager<User> userManager, IMapper mapper)
        {
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET api/expirationDates
        /// <summary>
        /// Returns paginated expiration dates
        /// </summary>
        /// <remarks>Expiration dates are returned in body, pagination parameters and other filters are returned in headers</remarks>
        /// <response code="200">Returns data</response>
        [HttpGet]
        public async Task<IActionResult> GetExpirationDates([FromQuery] ExpirationDatesFilterParams filterParams)
        {
            Expression<Func<ExpirationDate, object>>[] includings = new Expression<Func<ExpirationDate, object>>[2];

            if (filterParams.WithEmployees)
            {
                includings[0] = x => x.CollectedBy;
            }
            if (filterParams.WithProducts)
            {
                includings[1] = x => x.Product;
            }

            string userIdToFilter = (User.IsInRole("Admin") || User.IsInRole("Manager")) ? null : (await _userManager.GetUserAsync(User)).Id;

            PagedList<ExpirationDate> paginatedExpirationDates = await _repo.GetPaginated(filterParams.ExpirationDatesFiltersAsOneExpression(userIdToFilter), dbSet => dbSet.OrderBy(exp => exp.Collected ? 1 : 0).ThenBy(exp => exp.EndDate), filterParams, includings);

            Response.AddPagination(paginatedExpirationDates.CurrentPage, paginatedExpirationDates.PageSize, paginatedExpirationDates.TotalCount, paginatedExpirationDates.TotalPages);

            return Ok(_mapper.Map<IEnumerable<ExpirationDateVM>>(paginatedExpirationDates.Items));
        }

        // GET api/expirationDates/5
        /// <summary>
        /// Returns expiration date with informations about product to which it is assigned and informations about who collected products with this expiration date
        /// </summary>
        /// <response code="200">Expiration date found</response>
        /// <response code="404">No expiration date was found was found</response>
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
        /// <summary>
        /// Adds new expiration date to product
        /// </summary>
        /// <response code="201">Returns created expiration date</response>
        /// <response code="400">It this expiration date is already specified for selected product</response>
        /// <response code="500">Error occured while saving expiration date in db</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpirationDateCreateDTO expirationDate)
        {

            if (await _repo.Get(x => x.EndDate.Date == expirationDate.EndDate.Date && x.ProductId == expirationDate.ProductId) != null)
            {
                return BadRequest("This expiration date already exists for this product!");
            }

            var expirationDateEntity = _mapper.Map<ExpirationDate>(expirationDate);
            _repo.Add(expirationDateEntity);
            try
            {
                if (await _repo.SaveAll())
                {
                    return CreatedAtRoute("GetExpirationDate", new { id = expirationDateEntity.Id }, _mapper.Map<ExpirationDateForCreateVM>(expirationDateEntity));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


            return BadRequest("Error during adding new expiration date");
        }

        // PUT api/expirationDates/5
        /// <summary>
        /// To save collected products number from this expiration date.async User who sent request will be saved as collector.
        /// </summary>
        /// <response code="204">Expiration date updated - products collected</response>
        /// <response code="400">Id from route and from expiration date does not match or user wants to save number of collected items less than previously for the same date</response>
        /// <response code="404">Expiration date with specified id does not exist</response>
        /// <response code="500">Error occured while saving expiration date to db</response>
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

            try
            {
                if (await _repo.SaveAll())
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


            return BadRequest("Error during adding new expiration date");
        }

        // DELETE api/expirationDates/5
        /// <summary>
        /// Delete expiration date (added by mistake) - only for admins, managers and team leaders
        /// </summary>
        /// <response code="200">Expiration date deteled successfully</response>
        /// <response code="404">Expiration date with specified id does not exist</response>
        /// <response code="500">Error occured while deleting expiration date in db</response>
        [Authorize(Policy = "RequireMediumRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expirationDateToDelete = (await _repo.Get(x => x.Id == id));

            if (expirationDateToDelete == null)
            {
                return BadRequest("This expiration date doesn't exist!");
            }

            _repo.Delete(expirationDateToDelete);

            try{
            if (await _repo.SaveAll())
            {
                return Ok();
            }
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }


            return BadRequest("Error while trying to delete expiration date");
        }
    }
}