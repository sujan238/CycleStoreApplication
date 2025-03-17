using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CancellationFee { get; set; } = 0;

        public DateTime? WarrantyExpiry { get; set; }

        public string? InvoiceUrls { get; set; }

        // Navigation properties
        public Customer? Customer { get; set; }
        public User? Employee { get; set; }
        public ShippingDetails? ShippingDetails { get; set; }
        public DeliveryProof? DeliveryProof { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();
    }
}