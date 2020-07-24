using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.ExpirationDate
{
    public class ExpirationDateUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(0,255)]
        public int Count { get; set; }
    }
}