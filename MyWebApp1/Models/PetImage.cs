using MyWebApp1.Entities;

namespace MyWebApp1.Models
{
    public class PetImage
    {
        public int PetImageId { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public bool IsThumbnailImage { get; set; }
        public int PetId { get; set; }

        // Navigation property
        public Pet Pet { get; set; }
    }
}
