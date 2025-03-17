using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public int? SupplierId { get; set; } // Optional FK to Supplier table

        [Required]
        public int CycleId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // "Pending", "Ordered", "Received"

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Cycle? Cycle { get; set; }
    }
}