using System;

namespace RMS.Domain.Entities
{
    public class Tenant
    {
        // Notice this is a Guid because the Professor wanted high security for the Company ID
        public Guid Id { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Subdomain { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }

    }
}
