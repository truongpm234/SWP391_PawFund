using MyWebApp1.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyWebApp1.DTO
{
    public class AddNewPetDTO
    {
        public string? PetName { get; set; }

        public string? PetType { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string Address { get; set; }
        public string MedicalCondition { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        [ForeignKey("PetCategory")]
        public int? PetCategoryId { get; set; }
        public bool IsAdopted { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        [JsonIgnore]
        //public virtual PetCategory? PetCategory { get; set; }
        public PetCategory? PetCategory { get; set; }
        public List<PetImageDTO>? PetImages { get; set; }
    }
}