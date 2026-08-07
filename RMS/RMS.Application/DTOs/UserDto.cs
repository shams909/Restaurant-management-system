namespace RMS.Application.DTOs
{
    // This is the SAFE class we return to the internet!
    public class UserDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int RoleId { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }

        // Notice we COMPLETELY DELETED the Passcode and PasswordHash!
        // The internet is never allowed to see them!
    }
}
