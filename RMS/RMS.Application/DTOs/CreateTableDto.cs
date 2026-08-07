namespace RMS.Application.DTOs
{
    public class CreateTableDto
    {
        // Must provide the BranchId so we know which physical restaurant the table is in
        public int BranchId { get; set; }
        public string TableCode { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}
