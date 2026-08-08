using System.Collections.Generic;

namespace RMS.Application.DTOs
{
    public class DashboardDto
    {
        public decimal TotalRevenueToday { get; set; }
        public int TotalOrdersToday { get; set; }
        public List<string> LowStockAlerts { get; set; } = new List<string>();
    }
}
