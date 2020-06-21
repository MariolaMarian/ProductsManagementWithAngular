using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.User
{
    public class UserForLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}