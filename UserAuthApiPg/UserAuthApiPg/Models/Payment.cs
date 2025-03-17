using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty; // "CashOnPurchase", "UPIOnPurchase", "CashOnDelivery", "UPIOnDelivery", "HalfAdvance", "FullPayment"

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentStatus { get; set; } = "Pending"; // "Pending", "Paid", "PartiallyPaid", "Failed"

        public string? PaymentGatewayId { get; set; } // Stripe/Payment Gateway ID

        public DateTime? PaymentDate { get; set; }

        public bool IsRefunded { get; set; } = false;

        [Column(TypeName = "decimal(10,2)")]
        public decimal RefundAmount { get; set; } = 0;

        // Navigation property
        public Transaction? Transaction { get; set; }
    }
}