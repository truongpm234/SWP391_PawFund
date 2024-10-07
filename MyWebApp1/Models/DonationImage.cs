using System.ComponentModel.DataAnnotations;

namespace MyWebApp1.Models
{
    public class DonationImage
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageDescription { get; set; }
        public bool IsThumbnailImage { get; set; }
        public int DonationEventId { get; set; }

        // Navigation property
        public DonationEvent DonationEvent { get; set; }


    }
}
