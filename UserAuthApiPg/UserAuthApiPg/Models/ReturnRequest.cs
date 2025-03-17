using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class ReturnRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReturnId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"

        [Column(TypeName = "decimal(10,2)")]
        public decimal RefundAmount { get; set; } = 0;

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Transaction? Transaction { get; set; }
    }
}