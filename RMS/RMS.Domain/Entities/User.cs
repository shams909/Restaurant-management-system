namespace RMS.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        // Links the employee to a specific physical restaurant
        public int BranchId { get; set; }

        // Will link to the Roles table (e.g., Manager, Waiter)
        public int RoleId { get; set; }

        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public string Passcode { get; set; } // 4-digit pin for fast POS login
        public string PasswordHash { get; set; } // Secure password for web dashboard
    }
}
