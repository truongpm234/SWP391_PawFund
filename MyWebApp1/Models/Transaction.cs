using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public decimal TransactionAmount { get; set; }
        public int? DonationEventId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TransactionStatusId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

    }
}