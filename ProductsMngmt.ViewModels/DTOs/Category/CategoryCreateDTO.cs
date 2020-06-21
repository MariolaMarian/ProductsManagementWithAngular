using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}