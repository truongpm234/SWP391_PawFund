<<<<<<< HEAD
﻿using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations;
=======
﻿using System.ComponentModel.DataAnnotations;
>>>>>>> Dev-for-test
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
<<<<<<< HEAD

        public bool IsMoneyDonation { get; set; }

        public bool IsResourceDonation { get; set; }

=======
>>>>>>> Dev-for-test
        public int? DonationEventId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TransactionStatusId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

<<<<<<< HEAD
        // Navigation properties
       

=======
        [Required]
        public int ShelterId { get; set; } // Thêm trường ShelterId

        [MaxLength(200)]
        public string Note { get; set; } // Thêm trường Note

        [ForeignKey("ShelterId")]
        public Shelter Shelter { get; set; }
>>>>>>> Dev-for-test
    }
}
