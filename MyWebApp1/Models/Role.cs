using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Role")]

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
