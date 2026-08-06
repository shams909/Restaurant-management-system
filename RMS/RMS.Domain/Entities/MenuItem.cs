using System;

namespace RMS.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public int CategoryId { get; set; } // Links back to MenuCategory

        public string ItemCode { get; set; } // e.g., MNU-BRG-01
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
