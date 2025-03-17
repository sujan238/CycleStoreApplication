using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class DeliveryProof
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProofId { get; set; }

        [Required]
        public int ShippingId { get; set; }

        public string? ProofUrl { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        // Navigation properties
        public ShippingDetails? ShippingDetails { get; set; }
        public User? Employee { get; set; }
    }
}