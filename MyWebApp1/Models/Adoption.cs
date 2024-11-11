using MyWebApp1.Models;
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
        public string? FullName { get; set; }  
        public string? Address { get; set; }  
        public string? PhoneNumber { get; set; }  
        public string? Email { get; set; }  
        public string? SelfDescription { get; set; } 
        public bool? HasPetExperience { get; set; }
        public string? ReasonForAdopting { get; set; }  
        [JsonIgnore]
        public string? Reason { get; set; } 
    }
}
