using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.User
{
    public class UserToRegisterDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public bool IsManager { get; set; } = false;
        public bool IsTeamLeader { get; set; } = false;
        public List<int> Categories { get; set; }

    }
}