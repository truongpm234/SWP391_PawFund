using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.DTO
{
    public class ApproveAdoptionRequestDto
    {
        [Required]
        public bool IsApproved { get; set; }
        public string? Reason { get; set; }
    }

}
