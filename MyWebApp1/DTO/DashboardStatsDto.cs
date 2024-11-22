namespace MyWebApp1.DTO
{
    public class DashboardStatsDto
    {
        public int TotalAdoptionRequests { get; set; }
        public int ApprovedAdoptionRequests { get; set; }
        public int PendingAdoptionRequests { get; set; }
        public int RejectedAdoptionRequests { get; set; }
        public List<ShelterDonationDto> ShelterDonations { get; set; }
        public decimal TotalDonations { get; set; }
    }
}
