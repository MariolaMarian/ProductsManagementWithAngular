using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsMngmtAPI.Models
{
    public class ExpirationDate
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool Collected { get; set; }
        [Range(0, 255)]
        [DataType("tinyint")]
        public int? Count { get; set; }
        public DateTime CollectedDate { get; set; }
        [ForeignKey("CollectedBy")]
        public string CollectedById { get; set; }
        public virtual User CollectedBy { get; set; }
    }
}