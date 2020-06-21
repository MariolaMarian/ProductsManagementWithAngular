using Microsoft.AspNetCore.Identity;

namespace ProductsMngmt.BLL.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User {get;set;}
        public virtual Role Role {get;set;}
    }
}