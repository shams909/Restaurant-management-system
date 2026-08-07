using System;

namespace RMS.Application.DTOs
{
    public class CreateMenuCategoryDto
    {
        // No ID allowed! Let the database generate it.
        public Guid TenantId { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
    }
}
