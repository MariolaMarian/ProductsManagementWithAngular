using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsMngmtAPI.Models;

namespace ProductsMngmtAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetUsersWithAllIncludings()
        {
            return await GetUsersWithAllIncludingsAsQueryable().ToListAsync();
        }

        public async Task<User> GetUserWithAllIncludings(string id)
        {
            return await GetUsersWithAllIncludingsAsQueryable().SingleOrDefaultAsync(u => u.Id == id);
        }

        private IQueryable<User> GetUsersWithAllIncludingsAsQueryable()
        {
            return _userManager.Users.Include(u => u.UserRoles).ThenInclude(r => r.Role).Include(u => u.UserCategories).ThenInclude(uc => uc.Category);
        }
    }
}