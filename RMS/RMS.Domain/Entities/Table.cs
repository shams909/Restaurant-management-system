namespace RMS.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public int BranchId { get; set; } // Which branch this table is located in

        public string TableCode { get; set; } // e.g., TBL-A1
        public int Capacity { get; set; } // E.g., Seats 4 people
        public string Status { get; set; } // Available, Occupied, Reserved
    }
}
