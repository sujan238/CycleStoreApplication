using System.ComponentModel.DataAnnotations;

namespace UserAuthApiPg.Models
{
    public class CycleBrand
    {
        [Key] // Already unique as primary key
        public int BrandId { get; set; }

        [Required(ErrorMessage = "BrandName is required.")]
        [StringLength(100, ErrorMessage = "BrandName cannot exceed 100 characters.")]
        public string BrandName { get; set; }

        // Navigation property for Cycles
        public ICollection<Cycle> Cycles { get; set; } = new List<Cycle>();
    }
}