namespace RMS.Application.DTOs
{
    public class CreateTenantDto
    {
        // Notice there is no ID here. The user is NOT allowed to pick their own ID!
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Subdomain { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
    }
}
