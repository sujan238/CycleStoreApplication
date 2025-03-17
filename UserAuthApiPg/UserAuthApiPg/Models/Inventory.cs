using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }

        [Required]
        public int CycleId { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public string? StoreLocation { get; set; }

        [Required]
        public int StockQuantity { get; set; } = 0;

        public DateTime? LastRestockDate { get; set; } = DateTime.Now;

        // Navigation properties
        public Cycle? Cycle { get; set; }
    }
}