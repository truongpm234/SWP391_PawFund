using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table("UserApprove")]
    public class UserApprove
    {
        [Key]
        public int ApproveId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public string IDCardNumber { get; set; }
        public string PetCareCapacity { get; set; }
        public bool IsApprovedUser { get; set; }
        public DateTime? DateGet { get; set; }
        public string PlaceGet { get; set; }
        public string UsualAddress { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
