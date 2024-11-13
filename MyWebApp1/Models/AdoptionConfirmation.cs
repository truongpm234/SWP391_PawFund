using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    public class AdoptionConfirmation
    {
        [Key]
        public int ConfirmationId { get; set; } 

        public int AdoptionId { get; set; } 
        public Adoption Adoption { get; set; }

        public bool ConfirmationStatus { get; set; } = false;

        public DateTime? ConfirmationDate { get; set; }

        public bool ReminderSent { get; set; } = false;
    }

}
