namespace RMS.Application.DTOs
{
    // This is what we force the User to send us when they register
    public class CreateUserDto
    {
        // No ID allowed!
        public int BranchId { get; set; }
        public int RoleId { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }

        // We DO accept passwords here, because they are giving them to us to save!
        public string Passcode { get; set; }
        public string PasswordHash { get; set; }
    }
}
