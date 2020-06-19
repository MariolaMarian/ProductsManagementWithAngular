using System.ComponentModel.DataAnnotations;

namespace ProductsMngmtAPI.DTOs.Product
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Barecode { get; set; }
        public int MaxDays { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Image { get; set; }
    }
}