namespace RMS.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; } // e.g., The "Beef Burger"
        public int InventoryItemId { get; set; } // e.g., The "Raw Beef"

        public decimal QuantityUsed { get; set; } // e.g., 0.2 (Kg)
    }
}
