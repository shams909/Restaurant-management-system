using System;

namespace RMS.Domain.Entities
{
    public class Branch
    {
        // Notice this is an 'int' because the Professor wanted integers for everything except the Tenant
        public int Id { get; set; }

        // This links the Branch back to the Global Company
        public Guid TenantId { get; set; }

        public string BranchCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
    }
}
