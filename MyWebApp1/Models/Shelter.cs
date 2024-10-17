using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Shelter")]
    public class Shelter
    {
       
        public string ShelterLocation { get; set; }
        public string ShelterName { get; set; }
        public int Capacity { get; set; }
    }
}
