using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyWebApp1.Models
{
    [Table("Pet")]
    public class Pet
    {
        [Key]
        //[JsonIgnore]
        public int PetId { get; set; }
        public string? PetName { get; set; }
        public string? PetType { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? MedicalCondition { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? ContactEmail { get; set; }

        [ForeignKey("PetCategory")]
        public int? PetCategoryId { get; set; }

        public bool IsAdopted { get; set; }
        public bool IsApproved { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; } 
        [JsonIgnore]
        //public virtual PetCategory? PetCategory { get; set; }
        public PetCategory? PetCategory { get; set; }
        public int? ApprovedByUserId { get; set; }


        [ForeignKey("Shelter")]
        public int? ShelterId { get; set; }
        public virtual Shelter? ShelterName { get; set; }
        public virtual ICollection<PetImage>? PetImages { get; set; }
    }
}
