namespace RMS.Application.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CashRegisterId { get; set; }

        public string PaymentNo { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
