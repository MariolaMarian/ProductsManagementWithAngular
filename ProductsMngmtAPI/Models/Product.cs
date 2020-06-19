using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsMngmtAPI.Models
{
    public class Product
    {
        [Key]
        public int Id{get;set;}
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(13)]
        public string Barecode { get; set; }
        [DefaultValue(10)]
        [Range(1,255)]
        [DataType("tinyint")]
        public int MaxDays{get;set;}

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public byte[] Image{get;set;}
        public virtual ICollection<ExpirationDate> ExpirationDates {get;set;}

    }
}