namespace RMS.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        // [NEW] The Corporate Company they work for (Required)
        public System.Guid TenantId { get; set; }

        // The specific physical restaurant they work at
        // (We put a '?' to make it nullable, because the CEO/SuperAdmin doesn't work at a specific branch!)
        public int? BranchId { get; set; }

        // Will link to the Roles table (e.g., Manager, Waiter)
        public int RoleId { get; set; }

        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public string Passcode { get; set; } // 4-digit pin for fast POS login
        public string PasswordHash { get; set; } // Secure password for web dashboard
    }
}
