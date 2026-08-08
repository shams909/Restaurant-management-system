namespace RMS.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int BranchId { get; set; } // Which branch took the order

        public int? TableId { get; set; } // Nullable: Might be a takeaway order
        public int UserId { get; set; } // The waiter/cashier who took the order
        public int? CustomerId { get; set; } // Nullable: Might not be a loyalty member

        public string OrderNo { get; set; } // E.g., ORD-2023-11204
        public string OrderType { get; set; } // DineIn, Takeaway, Delivery
        public decimal GrandTotal { get; set; }
        public string Status { get; set; } // Open, Paid, Cancelled
         // [NEW] The time the order was placed!
        public System.DateTime OrderDate { get; set; } = System.DateTime.UtcNow;

        // Navigation property for Entity Framework to magically save the items with the order
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

