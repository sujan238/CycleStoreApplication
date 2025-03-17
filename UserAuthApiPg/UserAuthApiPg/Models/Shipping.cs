using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class ShippingDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShippingId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [Required]
        public int DeliveryEmployeeId { get; set; }

        [Required]
        public string CurrentStatus { get; set; } = "Order Placed";

        public string? StatusHistory { get; set; }

        [Required]
        public string TransportMode { get; set; } = "Van";

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryCharges { get; set; } = 5.00m;

        public DateTime? EstimatedDeliveryDate { get; set; }

        public string? TrackingNumber { get; set; }

        public DateTime? DeliveryDate { get; set; }

        // Navigation properties
        public Transaction? Transaction { get; set; }
        public User? DeliveryEmployee { get; set; }
    }
}