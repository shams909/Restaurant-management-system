namespace RMS.Application.DTOs
{
    // What the API sends back to the frontend upon a successful login
    public class AuthResponseDto
    {
        public string FullName { get; set; }
        public string Token { get; set; } // The VIP Pass!
    }
}
