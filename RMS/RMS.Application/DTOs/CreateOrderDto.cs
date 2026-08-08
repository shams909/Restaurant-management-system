using System.Collections.Generic;

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
        public string Status { get; set; }

        // [NEW]: The frontend only sends us WHAT they want to order!
        public List<OrderItemRequestDto> Items { get; set; }
    }

    // A tiny helper class just to receive the item requests
    public class OrderItemRequestDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
