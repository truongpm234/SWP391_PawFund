using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    namespace MyWebApp1.Models
    {
        [Table(name: "User")]
        public class User
        {
            [Key]
            public int UserId { get; set; }
            public string? Username { get; set; }
            public string? Fullname { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Address { get; set; }
            public bool IsApprovedUser { get; set; }
            public bool IsApproved { get; set; }
            public string Password { get; set; }
            public DateTime CreatedAt { get; set; }
            //public int RoleId { get; set; }
            [ForeignKey("Shelter")]
            public int? ShelterId { get; set; }

            public Shelter? Shelter { get; set; }

            [ForeignKey("Role")]
            public int? RoleId { get; set; }
            public Role? Role { get; set; }  // Điều hướng tới Role
            //public int? RoleId { get; set; }
            //public string? RoleName { get; set; }
            //public string RoleName { get; set; }
        }

    }

}