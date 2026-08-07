namespace RMS.Application.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int InventoryItemId { get; set; }
        public decimal QuantityUsed { get; set; }
    }
}
