using System;

namespace RMS.Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public string CustomerNo { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
