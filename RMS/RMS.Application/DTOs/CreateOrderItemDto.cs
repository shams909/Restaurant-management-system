namespace RMS.Application.DTOs
{
    public class CreateOrderItemDto
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int? VariantId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string KdsStatus { get; set; }
    }
}
