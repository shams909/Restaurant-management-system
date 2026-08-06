namespace RMS.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // The ticket being paid for
        public int CashRegisterId { get; set; } // Which register the cash went into

        public string PaymentNo { get; set; } // e.g., PAY-99382
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // Cash, CreditCard, Mobile
    }
}
