namespace RMS.Application.DTOs
{
    public class CreateRecipeDto
    {
        public int MenuItemId { get; set; }
        public int InventoryItemId { get; set; }
        public decimal QuantityUsed { get; set; }
    }
}
