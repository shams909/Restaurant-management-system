namespace RMS.Application.DTOs
{
    public class CreateOrderDto
    {
        public int BranchId { get; set; }
        public int? TableId { get; set; }
        public int UserId { get; set; }
        public int? CustomerId { get; set; }

        public string OrderNo { get; set; }
        public string OrderType { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; }
    }
}
