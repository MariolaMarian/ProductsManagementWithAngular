using System.ComponentModel.DataAnnotations;

namespace ProductsMngmtAPI.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}