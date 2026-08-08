namespace RMS.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Links to the main Order ticket

        public int MenuItemId { get; set; } // The exact food item ordered
        public int? VariantId { get; set; } // Nullable: E.g., The "Large" size

        public int BranchId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // This is extremely important for the Kitchen Display System (KDS)
        // Values will be: "Pending", "Cooking", "Ready", "Served"
        public string KdsStatus { get; set; }
    }
}
