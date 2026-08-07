using System;

namespace RMS.Application.DTOs
{
    public class MenuCategoryDto
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
    }
}
