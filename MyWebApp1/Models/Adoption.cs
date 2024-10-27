using MyWebApp1.Models; // Điều chỉnh namespace nếu cần thiết
using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyWebApp1.Models
{
    [Table(name: "Adoption")]
    public class Adoption
    {
        public int AdoptionId { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public bool IsApproved { get; set; }
        public string? Note { get; set; }

        // Thêm các thuộc tính mới
        public string? FullName { get; set; }  // Họ tên người nhận nuôi
        public string? Address { get; set; }  // Địa chỉ
        public string? PhoneNumber { get; set; }  // Số điện thoại
        public string? Email { get; set; }  // Email
        public string? SelfDescription { get; set; } // Miêu tả bản thân
        public bool? HasPetExperience { get; set; }// Đã từng nuôi pet chưa
        public string? ReasonForAdopting { get; set; }  // Lý do muốn nuôi pet
        [JsonIgnore]
        public string? Reason { get; set; } // Lý do từ chối có thể là null


        // Navigation properties
        //public User Username { get; set; } // Sửa tên từ Username sang User
        //public Pet PetName { get; set; } // Sửa tên từ PetName sang Pet
    }
}