using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;

namespace MyWebApp1.DTO
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public decimal TransactionAmount { get; set; }
        public int? DonationEventId { get; set; }
        public int UserId { get; set; }
        public int TransactionStatusId { get; set; }
        public int TransactionTypeId { get; set; }
        public int ShelterId { get; set; }
        public string Note { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ShelterName { get; set; }
        public User User { get; set; }
        public Shelter Shelter { get; set; }
    }

}
