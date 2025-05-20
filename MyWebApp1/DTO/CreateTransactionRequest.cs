public class CreateTransactionRequest
{
    public decimal TransactionAmount { get; set; }
    public bool IsMoneyDonation { get; set; }
    public bool IsResourceDonation { get; set; }
    public int TransactionTypeId { get; set; }
    public int ShelterId { get; set; } 
    public string Note { get; set; } 
}