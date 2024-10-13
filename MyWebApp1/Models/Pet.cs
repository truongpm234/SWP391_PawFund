using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyWebApp1.Models
{
    [Table("Pet")]
    public class Pet
    {
        [Key]
        [JsonIgnore]
        public int PetId { get; set; }

        public string PetName { get; set; }
        public string PetType { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string MedicalCondition { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        [ForeignKey("PetCategory")]
        public int PetCategoryId { get; set; }
        public bool IsAdopted { get; set; }
        public bool IsApproved { get; set; }
        [JsonIgnore]

        public virtual PetCategory? PetCategory { get; set; }
    }
}
