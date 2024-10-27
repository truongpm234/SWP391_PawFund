using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "DonationEvent")]
    public class DonationEvent
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        [StringLength(255)]
        public string EventName { get; set; }
        [Required]
        public string EventContent { get; set; }  // Không giới hạn độ dài
        [Required]
        public DateTime EventStartDate { get; set; }
        [Required]
        public DateTime EventEndDate { get; set; }
        public bool IsEnded { get; set; } = false;  // Mặc định là chưa kết thúc
        public bool IsApproved { get; set; } = false;  // Mặc định là chưa được duyệt
        [ForeignKey("UserCreated")]
        public int UserCreatedId { get; set; }
        // Navigation property
        public User UserCreated { get; set; }
    }
}
