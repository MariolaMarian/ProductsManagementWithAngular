using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProductsMngmt.BLL.Models
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles{get;set;}
    }
}