using System;

namespace RMS.Domain.Entities
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public int BranchId { get; set; }

        public int InventoryItemId { get; set; } // E.g., Burger Patty

        public string TransactionType { get; set; } // "Sale", "Restock", "Waste"
        public decimal QuantityChanged { get; set; } // E.g., -2

        public string Notes { get; set; } // E.g., "Sold in Order ORD-123"
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
