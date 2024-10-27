namespace MyWebApp1.DTO
{
    public class DonationListDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventContent { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public bool IsEnded { get; set; }
        public bool IsApproved { get; set; }
    }

}
