namespace RMS.Domain.Entities
{
    public class OrderItemAddon
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; } // Links to the specific line item
        public int AddonId { get; set; } // Links back to the ItemAddon table
    }
}
