using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.Category
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}