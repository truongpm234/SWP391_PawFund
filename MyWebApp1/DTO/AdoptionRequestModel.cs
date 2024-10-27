using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.DTO
{
    public class AdoptionRequestModel
    {
        //[Required]
        //public int PetId { get; set; }  // Pet cần nhận nuôi

        [Required]
        public string FullName { get; set; }  // Họ tên người nhận nuôi

        [Required]
        public string Address { get; set; }  // Địa chỉ

        [Required]
        public string PhoneNumber { get; set; }  // Số điện thoại

        [Required]
        public string Email { get; set; }  // Email

        [Required]
        public string SelfDescription { get; set; }  // Miêu tả bản thân

        [Required]
        public bool HasPetExperience { get; set; }  // Đã từng nuôi pet chưa

        [Required]
        public string ReasonForAdopting { get; set; }  // Lý do muốn nuôi pet

        public string Note { get; set; }  // Ghi chú thêm (nếu có)
    }
}
