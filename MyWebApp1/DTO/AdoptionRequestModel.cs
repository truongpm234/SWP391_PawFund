using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.DTO
{
    public class AdoptionRequestModel
    {
        //[Required]
        //public int PetId { get; set; }  
        [Required]
        public string FullName { get; set; }  

        [Required]
        public string Address { get; set; }  

        [Required]
        public string PhoneNumber { get; set; } 

        [Required]
        public string Email { get; set; }  

        [Required]
        public string SelfDescription { get; set; }  

        [Required]
        public bool HasPetExperience { get; set; }  

        [Required]
        public string ReasonForAdopting { get; set; } 

        public string Note { get; set; }  
        public DateTime createDate { get; set; }
    }
}
