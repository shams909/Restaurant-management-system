namespace RMS.Application.DTOs
{
    public class TableDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string TableCode { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}
