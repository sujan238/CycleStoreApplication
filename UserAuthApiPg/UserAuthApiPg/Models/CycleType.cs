using System.ComponentModel.DataAnnotations;

namespace UserAuthApiPg.Models
{
    public class CycleType
    {
        [Key] // Already unique as primary key
        public int TypeId { get; set; }

        [Required(ErrorMessage = "TypeName is required.")]
        [StringLength(100, ErrorMessage = "TypeName cannot exceed 100 characters.")]
        public string TypeName { get; set; }

        // Navigation property for Cycles
        public ICollection<Cycle> Cycles { get; set; } = new List<Cycle>();
    }
}