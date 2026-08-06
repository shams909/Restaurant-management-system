namespace RMS.Domain.Entities
{
    public class ItemAddon
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; } // Links directly to the specific MenuItem

        public string AddonCode { get; set; } // e.g., ADD-CHS
        public string Name { get; set; }
        public decimal Price { get; set; } // e.g., +1.00 for Extra Cheese
    }
}
