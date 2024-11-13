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
        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
