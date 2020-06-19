using System.ComponentModel.DataAnnotations;

namespace ProductsMngmtAPI.DTOs.Product
{
    public class ProductEditDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaxDays { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Image { get; set; }
    }
}