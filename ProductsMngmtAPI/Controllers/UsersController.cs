using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsMngmtAPI.Helpers.Pagination;
using ProductsMngmtAPI.Services.Users;
using ProductsMngmtAPI.VMs.User;

namespace ProductsMngmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize(Policy = "RequireHighestRole")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            var users = await _userService.GetUsersWithAllIncludings();

            var usersToReturn = _mapper.Map<IEnumerable<UserWithIncludingsVM>>(users);

            return Ok(usersToReturn.Where(u => !u.Roles.Contains("Admin")));
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserWithAllIncludings(id);

            if (user == null)
            {
                return BadRequest();
            }
            var userToReturn = _mapper.Map<UserWithIncludingsVM>(user);

            return Ok(userToReturn);
        }
    }
}
