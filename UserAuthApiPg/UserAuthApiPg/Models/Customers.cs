using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApiPg.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public long CustomerPhone { get; set; }

        [Required]
        public string CustomerAddress { get; set; } = string.Empty;

        [Required]
        public string CustomerCity { get; set; } = string.Empty;

        [Required]
        public string CustomerState { get; set; } = string.Empty;

        [Required]
        public long CustomerZip { get; set; }

        [Required]
        public string CustomerCountry { get; set; } = string.Empty;

        public string? CustomerNotes { get; set; }

        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}