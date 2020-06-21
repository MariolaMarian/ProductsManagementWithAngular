using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProductsMngmt.BLL.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName {get; set;}
        [Required]
        [StringLength(100)]
        public string LastName {get; set;}
        public virtual ICollection<UserCategory> UserCategories {get; set;}

        public virtual ICollection<UserRole> UserRoles {get; set;}
    }
}