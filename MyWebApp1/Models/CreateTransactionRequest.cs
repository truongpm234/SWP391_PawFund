// Models/CreateTransactionRequest.cs
namespace MyWebApp1.Models
{
    public class CreateTransactionRequest
    {
        public decimal TransactionAmount { get; set; }
        public bool IsMoneyDonation { get; set; }
        public bool IsResourceDonation { get; set; }
        public int UserId { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
