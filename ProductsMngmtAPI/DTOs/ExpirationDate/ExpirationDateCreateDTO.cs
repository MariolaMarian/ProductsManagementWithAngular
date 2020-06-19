using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsMngmtAPI.DTOs.ExpirationDate
{
    public class ExpirationDateCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}