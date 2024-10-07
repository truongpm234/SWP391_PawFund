using MyWebApp1.Entities;
using MyWebApp1.Models.MyWebApp1.Entities;

namespace MyWebApp1.Models
{
    public class Adoption
    {
        public int AdoptionId { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public bool IsApproved { get; set; }
        public string Note { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Pet Pet { get; set; }
    }
}
