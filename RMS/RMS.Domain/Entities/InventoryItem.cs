namespace RMS.Domain.Entities
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int BranchId { get; set; } // Inventory is tracked per physical branch!

        public string ItemCode { get; set; } // e.g., INV-BEEF-01
        public string Name { get; set; }
        public decimal CurrentStock { get; set; } // e.g., 50.5
        public string UnitOfMeasure { get; set; } // e.g., "Kg", "Pcs", "Liters"
        public decimal ReorderLevel { get; set; } // Alerts the manager when stock is too low
    }
}
