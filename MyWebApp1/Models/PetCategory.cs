using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    public class PetCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Category { get; set; }
        public string Row3 { get; set; }
    }
}
