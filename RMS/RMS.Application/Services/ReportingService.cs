using System;
using System.Linq;
using System.Threading.Tasks;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Application.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardDto> GetDailyDashboardAsync()
        {
            var dashboard = new DashboardDto();

            // 1. Get all Orders for today
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            var todaysOrders = orders.Where(o => o.OrderDate.Date == DateTime.UtcNow.Date).ToList();

            dashboard.TotalOrdersToday = todaysOrders.Count;
            dashboard.TotalRevenueToday = todaysOrders.Where(o => o.Status == "Paid").Sum(o => o.GrandTotal);

            // 2. Find Low Stock Items (Less than 10 left in the freezer!)
            var inventory = await _unitOfWork.Repository<InventoryItem>().GetAllAsync();
            var lowStockItems = inventory.Where(i => i.CurrentStock < 10).ToList();

            foreach (var item in lowStockItems)
            {
                dashboard.LowStockAlerts.Add($"WARNING: '{item.Name}' only has {item.CurrentStock} left in stock!");
            }

            return dashboard;
        }
    }
}
