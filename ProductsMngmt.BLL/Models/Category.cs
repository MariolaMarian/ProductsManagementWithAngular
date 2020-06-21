using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.BLL.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserCategory> CategoryUsers{get; set;}
    }
}