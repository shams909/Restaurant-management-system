using System;

namespace RMS.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }

        // Notice we are NOT sending 'CreatedAt' or the 'CompanyPhone' to the internet. 
        // We keep that private to our database!
    }
}
