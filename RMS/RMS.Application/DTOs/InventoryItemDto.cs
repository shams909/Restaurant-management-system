namespace RMS.Application.DTOs
{
    public class InventoryItemDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }

        public string ItemCode { get; set; }
        public string Name { get; set; }
        public decimal CurrentStock { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal ReorderLevel { get; set; }
    }
}
