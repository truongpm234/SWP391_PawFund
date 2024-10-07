using MyWebApp1.Models.MyWebApp1.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    public class DonationEvent
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventContent { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public bool IsEnded { get; set; }
        public bool IsApproved { get; set; }
        public int UserCreatedId { get; set; }

        // Navigation property
        public User UserCreated { get; set; }
    }
}
