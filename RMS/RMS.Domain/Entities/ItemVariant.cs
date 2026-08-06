namespace RMS.Domain.Entities
{
    public class ItemVariant
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; } // Links directly to the specific MenuItem

        public string VariantCode { get; set; } // e.g., VAR-LRG
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; } // e.g., +2.50 for Large
    }
}
