namespace RMS.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int? TableId { get; set; } // The '?' means Nullable (e.g., Takeout order has no table)
        public int UserId { get; set; }
        public int? CustomerId { get; set; }

        public string OrderNo { get; set; }
        public string OrderType { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; }
    }
}
