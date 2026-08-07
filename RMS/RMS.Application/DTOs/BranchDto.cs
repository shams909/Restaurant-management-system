using System;

namespace RMS.Application.DTOs
{
    public class BranchDto
    {
        public int Id { get; set; } // Fixed to 'int' instead of Guid!
        public Guid TenantId { get; set; }
        public string BranchCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
    }
}
