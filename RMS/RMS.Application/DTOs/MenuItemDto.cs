using System;

namespace RMS.Application.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public int CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
