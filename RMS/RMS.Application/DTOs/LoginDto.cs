namespace RMS.Application.DTOs
{
    // What the frontend sends to the API when a user tries to log in
    public class LoginDto
    {
        public string EmployeeNo { get; set; }
        public string Password { get; set; }
    }
}
