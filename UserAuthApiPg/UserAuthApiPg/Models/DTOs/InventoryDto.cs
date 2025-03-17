namespace UserAuthApiPg.Models.Dtos
{
    public class InventoryDto
    {
        public int InventoryId { get; set; }
        public int CycleId { get; set; }
        public string StoreLocation { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
}