using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "TransactionType")]
    public class TransactionType
    {
        public int TransactionTypeId { get; set; }
        public string Type { get; set; }
    }
}
