using System.ComponentModel.DataAnnotations;

namespace ProductsMngmt.ViewModels.DTOs.Email
{
    public class EmailForSentDTO
    {
        [Required]
        [DataType(DataType.Text)]
        public string Company{get;set;}

        [Required]
        [DataType(DataType.Text)]
        public string Owner{get;set;}

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone{get;set;}   

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Text)]
        public string Content {get;set;}
    }
}