using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductsMngmt.DAL.Repositories;
using ProductsMngmt.ViewModels.DTOs.User;
using ProductsMngmt.BLL.Models;
using ProductsMngmtAPI.Services.Users;
using ProductsMngmt.ViewModels.VMs.User;

namespace ProductsMngmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IRepository<UserCategory> _userCategoryRepo;
        private readonly IUserService _userService;

        public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IRepository<UserCategory> userCategoryRepo, IUserService userService)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _userCategoryRepo = userCategoryRepo;
            _userService = userService;
        }

        ///<summary>
        ///Allows to get JWT authorization token
        ///</summary>
        ///<param name="userForLoginDTO"></param>
        ///<response code="200">Returns token and user data if succesfully logged</response>
        ///<response code="401">Returns if user name or password was incorect</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenWithUserVM),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var existingUser = await _userManager.FindByNameAsync(userForLoginDTO.Username);

            if (existingUser != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(existingUser, userForLoginDTO.Password, false);
                if (result.Succeeded)
                {
                    //TO DO: return with categories and roles
                    var user = _mapper.Map<UserSimpleVM>(existingUser);

                    return Ok(new TokenWithUserVM
                    {
                        Token = GenerateJwtToken(existingUser).Result,
                        User = user
                    });
                }
            }

            return Unauthorized();
        }

        [Authorize(Policy = "RequireHighestRole")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDTO userToRegister)
        {
            var newUser = _mapper.Map<User>(userToRegister);

            var result = await _userManager.CreateAsync(newUser, userToRegister.Password);

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(newUser, "Employee").Wait();
                if (userToRegister.IsManager)
                {
                    _userManager.AddToRoleAsync(newUser, "Manager").Wait();
                    //add to all categories
                }
                if (userToRegister.IsTeamLeader)
                {
                    _userManager.AddToRoleAsync(newUser, "TeamLeader").Wait();
                }

                try
                {

                    if (userToRegister.Categories != null && userToRegister.Categories.Count > 0)
                    {

                        foreach (int categoryId in userToRegister.Categories)
                        {
                            _userCategoryRepo.Add(new UserCategory() { CategoryId = categoryId, UserId = newUser.Id });
                        }
                        await _userCategoryRepo.SaveAll();
                    }

                }
                catch (Exception)
                {
                    return BadRequest("Error when assigning user to categories");
                }

                var userToReturn = _mapper.Map<UserWithIncludingsVM>(await _userService.GetUserWithAllIncludings(newUser.Id));

                return CreatedAtRoute("GetUser", new { controller = "users", id = newUser.Id }, userToReturn );
            }

            return BadRequest(result.Errors);

        }

        [Authorize(Policy = "RequireHighestRole")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserData(UserToUpdateDTO user)
        {
            if (user.Id == null)
            {
                return BadRequest();
            }

            var userToUpdate = await _userManager.FindByIdAsync(user.Id);

            if (userToUpdate == null)
            {
                return BadRequest();
            }

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.PhoneNumber = user.PhoneNumber;

            try
            {
                if (!(await _userManager.IsInRoleAsync(userToUpdate, "Employee")))
                    _userManager.AddToRoleAsync(userToUpdate, "Employee").Wait();

                if (user.IsManager)
                {
                    _userManager.AddToRoleAsync(userToUpdate, "Manager").Wait();
                    //add to all categories
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(userToUpdate, "Manager"))
                        _userManager.RemoveFromRoleAsync(userToUpdate, "Manager").Wait();
                }
                if (user.IsTeamLeader)
                {
                    _userManager.AddToRoleAsync(userToUpdate, "TeamLeader").Wait();
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(userToUpdate, "TeamLeader"))
                        _userManager.RemoveFromRoleAsync(userToUpdate, "TeamLeader").Wait();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            try
            {
                await _userCategoryRepo.Delete(uc => uc.UserId == user.Id);

                if (user.Categories != null && user.Categories.Count > 0)
                {

                    foreach (int categoryId in user.Categories)
                    {
                        _userCategoryRepo.Add(new UserCategory() { CategoryId = categoryId, UserId = user.Id });
                    }
                    await _userCategoryRepo.SaveAll();
                }

            }
            catch (Exception)
            {
                return BadRequest("Error when assigning user to categories");
            }

            var result = await _userManager.UpdateAsync(userToUpdate);

            if (result.Succeeded)
            {
                return Ok(_mapper.Map<UserWithIncludingsVM>(await _userService.GetUserWithAllIncludings(userToUpdate.Id)));
            }

            else
            {
                return BadRequest();
            }
        }

        /*
        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(User, changePasswordDTO.OldPassword, false);
            if (result.Succeeded)
            {
                //send notification email
                return Ok();
            }
        }
        */


        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}