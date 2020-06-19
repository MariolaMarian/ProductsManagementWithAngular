using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsMngmtAPI.DTOs.User
{
    public class UserToUpdateDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public bool IsManager { get; set; } = false;
        public bool IsTeamLeader { get; set; } = false;
        public List<int> Categories { get; set; }
    }
}