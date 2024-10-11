using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    public class AdoptionRequestModel
    {
        public int UserId { get; set; }

        [Required]
        public int PetId { get; set; }

        public string Note { get; set; }
    }
}
