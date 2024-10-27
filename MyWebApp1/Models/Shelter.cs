using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Shelter")]
    public class Shelter
    {
        [Key]
        public int ShelterId { get; set; }  
        public string ShelterLocation { get; set; }
        public string ShelterName { get; set; }
        public int Capacity { get; set; }
        [NotMapped]

        public User? User { get; set; }
    }
}