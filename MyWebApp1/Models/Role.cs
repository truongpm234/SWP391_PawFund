using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Role")]

    public class Role
    {
        public int RoleId { get; set; } // Hoặc tên khác tùy theo thiết kế của bạn
        public string Name { get; set; } // Tên vai trò
    }
}
