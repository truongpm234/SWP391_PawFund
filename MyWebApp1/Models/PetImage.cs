using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "PetImage")]

    public class PetImage
    {
        public int PetImageId { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public bool IsThumbnailImage { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
