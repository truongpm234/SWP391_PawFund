using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "DonationEvent")]

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
