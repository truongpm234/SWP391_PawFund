using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    namespace MyWebApp1.Entities
    {
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public bool IsApprovedUser { get; set; }
            public bool IsApproved { get; set; }
            public string Password { get; set; }
            public DateTime CreatedAt { get; set; }
        }

    }

}
