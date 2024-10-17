using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Adoption")]

    public class Adoption
    {
        public int AdoptionId { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public bool IsApproved { get; set; }
        public string Note { get; set; }

        // Navigation properties
        public User Username { get; set; }
        public Pet PetName { get; set; }
    }
}
