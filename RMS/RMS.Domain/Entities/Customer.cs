using System;

namespace RMS.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        // Customers belong to the Global Company so they can use their loyalty points at ANY branch!
        public Guid TenantId { get; set; }

        public string CustomerNo { get; set; } // e.g., CUST-10045
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
