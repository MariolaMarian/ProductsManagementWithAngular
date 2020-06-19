using Microsoft.AspNetCore.Identity;

namespace ProductsMngmtAPI.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User {get;set;}
        public virtual Role Role {get;set;}
    }
}