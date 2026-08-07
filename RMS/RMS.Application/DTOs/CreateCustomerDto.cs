using System;

namespace RMS.Application.DTOs
{
    public class CreateCustomerDto
    {
        public Guid TenantId { get; set; }
        public string CustomerNo { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
