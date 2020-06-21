using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.ExpirationDate
{
    public class ExpirationDateCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}