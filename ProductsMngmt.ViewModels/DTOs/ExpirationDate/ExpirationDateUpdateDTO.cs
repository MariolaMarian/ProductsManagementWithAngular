using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.ExpirationDate
{
    public class ExpirationDateUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
    }
}