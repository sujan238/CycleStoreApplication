namespace UserAuthApiPg.Models.Dtos
{
    public class CycleDto
    {
        public int CycleId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string CycleBrandName { get; set; } = string.Empty;
        public string CycleTypeName { get; set; } = string.Empty;
        public int BrandId { get; set; } // Added for mapping
        public int TypeId { get; set; }  // Added for mapping
        public decimal CyclePrice { get; set; }
        public decimal CycleDeliveryCharges { get; set; }
        public string CycleColor { get; set; } = "Black";
        public string CycleSize { get; set; } = "Standard"; // Added
        public string? ImageUrl { get; set; }
        public int TotalStock { get; set; }
    }

    public class CycleUpdateDto
    {
        public int CycleId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public decimal CyclePrice { get; set; }
        public decimal CycleDeliveryCharges { get; set; }
        public string CycleColor { get; set; } = "Black";
        public string CycleSize { get; set; } = "Standard"; // Added
    }
}