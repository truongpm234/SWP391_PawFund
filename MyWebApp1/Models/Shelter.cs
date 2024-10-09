using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Shelter")]
    public class Shelter
    {
       
        public string shelterLocation { get; set; }
        public string shelterName { get; set; }
        public int capacity { get; set; }
    }
}
