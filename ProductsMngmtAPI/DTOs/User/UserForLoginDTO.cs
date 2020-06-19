using System.ComponentModel.DataAnnotations;

namespace ProductsMngmtAPI.DTOs.User
{
    public class UserForLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}