using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class Cycle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CycleId { get; set; }

        [Required]
        public string ModelName { get; set; } = string.Empty;

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryCharges { get; set; } = 5.00m;

        public bool IsCustomizable { get; set; } = false;

        [Required]
        public string Color { get; set; } = "Black";

        public string? Size { get; set; } // Added

        public string? ImageUrl { get; set; }

        // Navigation properties
        public CycleBrand? Brand { get; set; }
        public CycleType? Type { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}