using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.DTO
{
    public class ApproveAdoptionRequestDto
    {
        [Required]
        public int IsApproved { get; set; }
        public string? Reason { get; set; }
    }

}
