using System;

namespace RMS.Application.DTOs
{
    public class CreateBranchDto
    {
        public Guid TenantId { get; set; }
        public string BranchCode { get; set; } // Added this!
        public string Name { get; set; }       // Fixed name
        public string Address { get; set; }
        public string ContactPhone { get; set; } // Fixed name
    }
}
