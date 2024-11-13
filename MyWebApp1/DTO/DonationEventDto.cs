using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.DTO
{
    public class DonationEventDto
    {
        [Required]
        public string EventName { get; set; }

        [Required]
        public string EventContent { get; set; } 

        [Required]
        public DateTime EventStartDate { get; set; }

        [Required]
        public DateTime EventEndDate { get; set; }
    }

}
