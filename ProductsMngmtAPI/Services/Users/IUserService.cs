using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsMngmtAPI.Models;

namespace ProductsMngmtAPI.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersWithAllIncludings();
        Task<User> GetUserWithAllIncludings(string id);
    }
}