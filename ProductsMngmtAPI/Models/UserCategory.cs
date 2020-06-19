using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ProductsMngmtAPI.Models
{
    public class UserCategory
    {
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}