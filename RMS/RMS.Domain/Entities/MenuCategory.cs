using System;

namespace RMS.Domain.Entities
{
    public class MenuCategory
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; } // Shared across all branches of the company

        public string CategoryCode { get; set; } // e.g., CAT-BEV
        public string Name { get; set; }
    }
}
