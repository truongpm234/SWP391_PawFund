using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "PetCategory")]

    public class PetCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetCategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
